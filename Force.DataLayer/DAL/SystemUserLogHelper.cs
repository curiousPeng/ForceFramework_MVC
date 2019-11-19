/*
 *  2019-11-18 10:47:41
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using DataLayer.Base;
using Force.Model;
using Force.DataLayer.Metadata;

namespace Force.DataLayer
{
	namespace Metadata
	{
		public sealed class SystemUserLogColumn : IColumn
		{
			internal SystemUserLogColumn(string table, string name)
			{
				Table = table;
				Name = name;
			}

			public string Name { private set; get; }

			public string Table { private set; get; }

			public bool IsAddEqual { private set; get; }

			private bool _asc;
			public string Asc { get { return this._asc ? "ASC" : "DESC"; } }

			public SystemUserLogColumn SetAddEqual() { IsAddEqual ^= true; return this; }

			public SystemUserLogColumn SetOrderByAsc() { this._asc = true; return this; }

			public SystemUserLogColumn SetOrderByDesc() { this._asc = false; return this; }
		}

		public sealed class SystemUserLogTable
		{
			internal SystemUserLogTable(string name)
			{
				Name = name;
			}

			public string Name { private set; get; }
		}
	}

	public partial class SystemUserLogHelper : BaseTableHelper
	{
		public static readonly SystemUserLogTable Table = new SystemUserLogTable("SystemUserLog");

		public sealed class Columns
		{
			public static readonly SystemUserLogColumn Id = new SystemUserLogColumn("SystemUserLog", "Id");
			public static readonly SystemUserLogColumn SystemUserId = new SystemUserLogColumn("SystemUserLog", "SystemUserId");
			public static readonly SystemUserLogColumn SystemUserName = new SystemUserLogColumn("SystemUserLog", "SystemUserName");
			public static readonly SystemUserLogColumn ActionRoute = new SystemUserLogColumn("SystemUserLog", "ActionRoute");
			public static readonly SystemUserLogColumn Details = new SystemUserLogColumn("SystemUserLog", "Details");
			public static readonly SystemUserLogColumn Type = new SystemUserLogColumn("SystemUserLog", "Type");
			public static readonly SystemUserLogColumn IP = new SystemUserLogColumn("SystemUserLog", "IP");
			public static readonly SystemUserLogColumn CreatedTime = new SystemUserLogColumn("SystemUserLog", "CreatedTime");
			public static readonly List<SystemUserLogColumn> All = new List<SystemUserLogColumn> { Id, SystemUserId, SystemUserName, ActionRoute, Details, @Type, IP, CreatedTime };
		}

		/// <summary>
		/// 是否存在指定的SystemUserLog实体对象
		/// </summary>
		/// <param name="Id">Id</param>
		/// <returns>是否存在，true为存在</returns>
		public static bool Exists(int Id)
		{
			var sql = new StringBuilder();
			sql.Append("SELECT COUNT(1) FROM [SystemUserLog]");
			sql.Append(" WHERE [Id]=@Id ");
			var ret = false;
			using (var conn = GetOpenConnection())
			{
				ret = conn.ExecuteScalar<int>(sql.ToString(), new { @Id=Id }) > 0;
			}

			return ret;
		}

		/// <summary>
		/// 是否存在指定的SystemUserLog实体对象
		/// </summary>
		public static bool Exists(Expression<Func<SystemUserLog, bool>> predicate)
		{
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUserLog]");
            sql.Append(" WHERE " + where_str);
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString()) > 0;
            }

            return ret;
		}

		/// <summary>
        /// 添加新SystemUserLog记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
        /// <returns>新插入数据的id</returns>
        public static int Insert(SystemUserLog model, SqlConnection conn = null, SqlTransaction transaction = null)
        {
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [SystemUserLog]([SystemUserId], [SystemUserName], [ActionRoute], [Details], [Type], [IP], [CreatedTime])");
            sql.Append(" OUTPUT INSERTED.[Id] ");
            sql.Append("VALUES(@SystemUserId, @SystemUserName, @ActionRoute, @Details, @Type, @IP, @CreatedTime)");
            var ret = 0;
            if (conn != null)
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException("transaction");
                }
                ret = conn.ExecuteScalar<int>(sql.ToString(), model, transaction);
            }
            else
            {
                using (var conn1 = GetOpenConnection())
                {
                    ret = conn1.ExecuteScalar<int>(sql.ToString(), model);
                }
            }

            return ret;
        }

		/// <summary>
        /// 删除指定的SystemUserLog数据记录
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserLog] ");
            sql.Append(" WHERE [Id]=@Id ");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @Id=Id }) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 删除指定的SystemUserLog数据记录
        /// </summary>
        public static bool Delete(Expression<Func<SystemUserLog, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}
			
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserLog]");
			if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString()) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 批量删除指定的SystemUserLog数据记录
        /// </summary>
        /// <param name="Ids">SystemUserLog实体对象的id列表</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(List<int> Ids)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserLog] ");
            sql.Append(" WHERE [Id] IN @ids");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @ids = Ids}) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: null, 
				reverse: false, 
				conn: null, 
				transaction: null, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, bool reverse, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: null, 
				reverse: reverse, 
				conn: null, 
				transaction: null, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, SqlConnection conn, SqlTransaction transaction, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: null, 
				reverse: false, 
				conn: conn, 
				transaction: transaction, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: null, 
				reverse: reverse, 
				conn: conn, 
				transaction: transaction, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, Expression<Func<SystemUserLog, bool>> predicate, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: predicate, 
				reverse: false, 
				conn: null, 
				transaction: null, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, Expression<Func<SystemUserLog, bool>> predicate, bool reverse, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: predicate, 
				reverse: reverse, 
				conn: null, 
				transaction: null, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, Expression<Func<SystemUserLog, bool>> predicate, SqlConnection conn, SqlTransaction transaction, params SystemUserLogColumn[] fields)
        {
            return Update(
				model: model, 
				predicate: predicate, 
				reverse: false, 
				conn: conn, 
				transaction: transaction, 
				fields: fields);
        }

		/// <summary>
        /// 更新SystemUserLog数据记录
        /// </summary>
        /// <param name="model">SystemUserLog实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserLog model, Expression<Func<SystemUserLog, bool>> predicate, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserLogColumn[] fields)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}
			else
			{
				where_str = "[Id] = @Id";
			}

            var sql = new StringBuilder();
            sql.Append("UPDATE [SystemUserLog]");
            if (fields == null || fields.Length == 0)
            {
                 sql.Append(" SET [SystemUserId] = @SystemUserId, [SystemUserName] = @SystemUserName, [ActionRoute] = @ActionRoute, [Details] = @Details, [Type] = @Type, [IP] = @IP, [CreatedTime] = @CreatedTime");
            }
            else
            {
				if (reverse == true)
				{
					 fields = (SystemUserLogColumn[])Columns.All.Except(fields);
				}
                sql.Append(" SET ");
                for (int i = 0; i < fields.Length; i++)
                {
					if (fields[i].IsAddEqual)
					{
						sql.Append("[" + fields[i].Name + "] += @" + fields[i].Name + "");
						fields[i].SetAddEqual();
					}
					else
					{
						sql.Append("[" + fields[i].Name + "] = @" + fields[i].Name + "");
					}
                    
                    if (i != fields.Length - 1)
                    {
                        sql.Append(",");
                    }
                }
            }
            sql.Append(" WHERE " + where_str);
            var ret = false;
            if (conn != null)
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException("transaction");
                }
                ret = conn.Execute(sql.ToString(), model, transaction) > 0;
            }
            else
            {
                using (var conn1 = GetOpenConnection())
                {
                    ret = conn1.Execute(sql.ToString(), model) > 0;
                }
            }

            return ret;
        }

		/// <summary>
        /// 获取指定的SystemUserLog实体对象
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>SystemUserLog实体</returns>
        public static SystemUserLog GetModel(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 [Id], [SystemUserId], [SystemUserName], [ActionRoute], [Details], [Type], [IP], [CreatedTime] FROM [SystemUserLog] ");
            sql.Append(" WHERE [Id]=@Id ");
            SystemUserLog ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUserLog>(sql.ToString(), new { @Id=Id });
            }

            return ret;
        }

		/// <summary>
        /// 获取指定的SystemUserLog实体对象
        /// </summary>
        public static SystemUserLog GetModel(Expression<Func<SystemUserLog, bool>> predicate)
        {
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);
			
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 * FROM [SystemUserLog] ");
            sql.Append(" WHERE " + where_str);
            SystemUserLog ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUserLog>(sql.ToString());
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUserLog实体对象
        /// </summary>
        public static List<SystemUserLog> GetList(Expression<Func<SystemUserLog, bool>> predicate = null, params SystemUserLogColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT * FROM [SystemUserLog]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUserLog> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUserLog>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUserLog实体对象
        /// </summary>
        public static List<SystemUserLog> GetList(int top, Expression<Func<SystemUserLog, bool>> predicate = null, params SystemUserLogColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT TOP " + top.ToString() + " * FROM [SystemUserLog]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUserLog> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUserLog>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
		/// 获取记录总数
		/// </summary>
        public static int GetCount(Expression<Func<SystemUserLog, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUserLog]");
			if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);

            var ret = -1;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString());
            }

            return ret;
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public static PageDataView<SystemUserLog> GetPage(
			Expression<Func<SystemUserLog, bool>> predicate = null, 
			int pageSize = 20, 
			int currentPage = 1, 
			SystemUserLogColumn orderBy = null, 
			params SystemUserLogColumn[] columns)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

			var orderby_str = string.Empty;
            if (orderBy != null)
            {
                orderby_str = $"[{orderBy.Name}] {orderBy.Asc}";
            }

            var column_str = "*";
            if (columns != null && columns.Length > 0)
            {
                column_str = string.Join(", ", columns.Select(p => $"[{p.Name}]"));
            }

			return Paged<SystemUserLog>(
					"[SystemUserLog]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public static PageDataView<SystemUserLog> GetPage(
			Expression<Func<SystemUserLog, bool>> predicate, 
			int pageSize, 
			int currentPage, 
			IList<SystemUserLogColumn> orderBy, 
			params SystemUserLogColumn[] columns)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

			var orderby_str = string.Empty;
            if (orderBy != null && orderBy.Count > 0)
            {
                orderby_str = string.Join(", ", orderBy.Select(p => $"[{p.Name}] {p.Asc}"));
            }

            var column_str = "*";
            if (columns != null && columns.Length > 0)
            {
                column_str = string.Join(", ", columns.Select(p => $"[{p.Name}]"));
            }

			return Paged<SystemUserLog>(
					"[SystemUserLog]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }
	}
}
