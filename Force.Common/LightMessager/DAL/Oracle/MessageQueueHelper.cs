﻿using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Force.Common.LightMessager.DAL.Model;
using Oracle.ManagedDataAccess.Client;

namespace Force.Common.LightMessager.DAL.Oracle
{
    public class MessageQueueHelper : IMessageQueueHelper
    {
        private readonly string _connectstring;
        public MessageQueueHelper(string connectionString)
        {
            _connectstring = connectionString;
        }
        private OracleConnection GetOpenConnection()
        {
            var connection = new OracleConnection(_connectstring);
            connection.Open();
            return connection;
        }
        public MessageQueue GetModelBy(long msgHash)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT \"Id\", \"MsgHash\", \"MsgContent\", \"CanBeRemoved\", \"RetryCount\", \"LastRetryTime\", \"CreatedTime\" FROM \"MessageQueue\" ");
            sql.Append(" WHERE \"MsgHash\"=:MsgHash");
            MessageQueue ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<MessageQueue>(sql.ToString(), new { @MsgHash = msgHash });
            }

            return ret;
        }

        public int Insert(MessageQueue model)
        {
            var sql = new StringBuilder();
            sql.Append("INSERT INTO \"MessageQueue\"(\"MsgHash\", \"MsgContent\", \"Status\", \"RetryCount\", \"LastRetryTime\", \"CanBeRemoved\", \"CreatedTime\")");
            sql.Append(" OUTPUT INSERTED.\"Id\" ");
            sql.Append("VALUES(:MsgHash, :MsgContent, :Status, :RetryCount, :LastRetryTime, :CanBeRemoved, :CreatedTime)");
            var ret = 0;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString(), model);
            }

            return ret;
        }

        public bool Update(long msgHash, short fromStatus, short toStatus)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE \"MessageQueue\" ");
            sql.AppendLine("SET \"Status\"=:toStatus, \"RetryCount\"=\"RetryCount\"+1, \"LastRetryTime\"=:LastRetryTime, \"CanBeRemoved\"=:CanBeRemoved ");
            sql.AppendLine("WHERE \"MsgHash\"=:MsgHash and \"Status\"=:fromStatus");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                var retVal = conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    @MsgHash = msgHash,
                    @fromStatus = fromStatus,
                    @toStatus = toStatus,
                    @LastRetryTime = DateTime.Now,
                    @CanBeRemoved = toStatus == 5 ? true : false // 5 Processed
                });
                if (retVal == 0)
                {
                    var sql1 = "UPDATE \"MessageQueue\" set \"Status\"=4 WHERE \"MsgHash\"=:MsgHash"; // 4 Exception
                    conn.Execute(sql1, new
                    {
                        @MsgHash = msgHash
                    });
                }

                ret = retVal > 0;
            }

            return ret;
        }

        public bool Update(long msgHash, short fromStatus1, short fromStatus2, short toStatus)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE \"MessageQueue\" ");
            sql.AppendLine("SET \"Status\"=:toStatus, \"RetryCount\"=\"RetryCount\"+1, \"LastRetryTime\"=:LastRetryTime, \"CanBeRemoved\"=:CanBeRemoved ");
            sql.AppendLine("WHERE \"MsgHash\"=:MsgHash and (\"Status\"=:fromStatus1 or \"Status\"=:fromStatus1)");

            var ret = false;
            using (var conn = GetOpenConnection())
            {
                var retVal = conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    @MsgHash = msgHash,
                    @fromStatus1 = fromStatus1,
                    @fromStatus2 = fromStatus2,
                    @toStatus = toStatus,
                    @LastRetryTime = DateTime.Now,
                    @CanBeRemoved = toStatus == 5 ? true : false // 5 Processed
                });
                if (retVal == 0)
                {
                    var sql1 = "UPDATE \"MessageQueue\" set \"Status\"=4 WHERE \"MsgHash\"=:MsgHash"; // 4 Exception
                    conn.Execute(sql1, new
                    {
                        @MsgHash = msgHash
                    });
                }
                ret = retVal > 0;
            }

            return ret;
        }

        public bool UpdateCanbeRemoveIsFalse(long msgHash)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE \"MessageQueue\" ");
            sql.AppendLine("SET \"CanBeRemoved\"=:CanBeRemoved ");
            sql.AppendLine("WHERE \"MsgHash\"=:MsgHash");

            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    @MsgHash = msgHash,
                    @CanBeRemoved = true
                }) > 0;
            }

            return ret;
        }
    }
}
