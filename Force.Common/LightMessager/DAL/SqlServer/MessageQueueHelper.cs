using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Force.Common.LightMessager.DAL.Model;

namespace Force.Common.LightMessager.DAL.SqlServer
{
    public class MessageQueueHelper : IMessageQueueHelper
    {
        private readonly string _connectstring;
        public MessageQueueHelper(string connectionString) {
            _connectstring = connectionString;
        }
        private SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = _connectstring;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };
                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public MessageQueue GetModelBy(long msgHash)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 [Id], [MsgHash], [MsgContent], [CanBeRemoved], [RetryCount], [LastRetryTime], [CreatedTime] FROM [MessageQueue] ");
            sql.Append(" WHERE [MsgHash]=@MsgHash");
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
            sql.Append("INSERT INTO [MessageQueue]([MsgHash], [MsgContent], [Status], [RetryCount], [LastRetryTime], [CanBeRemoved], [CreatedTime])");
            sql.Append(" OUTPUT INSERTED.[Id] ");
            sql.Append("VALUES(@MsgHash, @MsgContent, @Status, @RetryCount, @LastRetryTime, @CanBeRemoved, @CreatedTime)");
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
            sql.AppendLine("DECLARE @retVal int ");
            sql.AppendLine("UPDATE [MessageQueue] ");
            sql.AppendLine("SET [Status]=@toStatus, [RetryCount]=[RetryCount]+1, [LastRetryTime]=@LastRetryTime, [CanBeRemoved]=@CanBeRemoved ");
            sql.AppendLine("WHERE [MsgHash]=@MsgHash and [Status]=@fromStatus");
            sql.AppendLine("SELECT @retVal = @@Rowcount ");
            sql.AppendLine("IF (@retVal = 0) BEGIN");
            sql.AppendLine("UPDATE [MessageQueue] set [Status]=5 WHERE [MsgHash]=@MsgHash END SELECT @retVal"); // 5 Exception
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    @MsgHash = msgHash,
                    @fromStatus = fromStatus,
                    @toStatus = toStatus,
                    @LastRetryTime = DateTime.Now,
                    @CanBeRemoved = toStatus == 6 ? true : false // 6 Processed
                }) > 0;
            }

            return ret;
        }

        public bool Update(long msgHash, short fromStatus1, short fromStatus2, short toStatus)
        {
            var sql = new StringBuilder();
            sql.AppendLine("DECLARE @retVal int ");
            sql.AppendLine("UPDATE [MessageQueue] ");
            sql.AppendLine("SET [Status]=@toStatus, [RetryCount]=[RetryCount]+1, [LastRetryTime]=@LastRetryTime, [CanBeRemoved]=@CanBeRemoved ");
            sql.AppendLine("WHERE [MsgHash]=@MsgHash and ([Status]=@fromStatus1 or [Status]=@fromStatus1)");
            sql.AppendLine("SELECT @retVal = @@Rowcount ");
            sql.AppendLine("IF (@retVal = 0) BEGIN");
            sql.AppendLine("UPDATE [MessageQueue] set [Status]=5 WHERE [MsgHash]=@MsgHash END SELECT @retVal"); // 5 Exception
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    @MsgHash = msgHash,
                    @fromStatus1 = fromStatus1,
                    @fromStatus2 = fromStatus2,
                    @toStatus = toStatus,
                    @LastRetryTime = DateTime.Now,
                    @CanBeRemoved = toStatus == 6 ? true : false // 6 Processed
                }) > 0;
            }

            return ret;
        }

        public bool UpdateCanbeRemoveIsFalse(long msgHash)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE [MessageQueue] ");
            sql.AppendLine("SET [CanBeRemoved]=@CanBeRemoved ");
            sql.AppendLine("WHERE [MsgHash]=@MsgHash");

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
