/*
 *  2019-03-12 16:13:58
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using Dapper;
using Force.DataLayer.Base;
using Force.DataLayer.Metadata;
using Force.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Force.DataLayer
{
    namespace Metadata
    {
        public sealed class SystemUserColumn : IColumn
		{
			internal SystemUserColumn(string table, string name)
			{
				Table = table;
				Name = name;
			}

			public string Name { private set; get; }

			public string Table { private set; get; }

			public bool IsAddEqual { private set; get; }

			private bool _asc;
			public string Asc { get { return this._asc ? "ASC" : "DESC"; } }

			public SystemUserColumn SetAddEqual() { IsAddEqual ^= true; return this; }

			public SystemUserColumn SetOrderByAsc() { this._asc = true; return this; }

			public SystemUserColumn SetOrderByDesc() { this._asc = false; return this; }
		}

		public sealed class SystemUserTable
		{
			internal SystemUserTable(string name)
			{
				Name = name;
			}

			public string Name { private set; get; }
		}
	}

	public partial class SystemUserHelper : BaseTableHelper
	{
		public static readonly SystemUserTable Table = new SystemUserTable("SystemUser");

		public sealed class Columns
		{
			public static readonly SystemUserColumn Id = new SystemUserColumn("SystemUser", "Id");
			public static readonly SystemUserColumn Account = new SystemUserColumn("SystemUser", "Account");
			public static readonly SystemUserColumn Email = new SystemUserColumn("SystemUser", "Email");
			public static readonly SystemUserColumn Password = new SystemUserColumn("SystemUser", "Password");
			public static readonly SystemUserColumn Phone = new SystemUserColumn("SystemUser", "Phone");
			public static readonly SystemUserColumn HeadImage = new SystemUserColumn("SystemUser", "HeadImage");
			public static readonly SystemUserColumn Status = new SystemUserColumn("SystemUser", "Status");
			public static readonly SystemUserColumn CreatedTime = new SystemUserColumn("SystemUser", "CreatedTime");
			public static readonly List<SystemUserColumn> All = new List<SystemUserColumn> { Id, Account, Email, Password, Phone, HeadImage, Status, CreatedTime };
		}

		/// <summary>
		/// 是否存在指定的SystemUser实体对象
		/// </summary>
		/// <param name="Id">Id</param>
		/// <returns>是否存在，true为存在</returns>
		public static bool Exists(int Id)
		{
			var sql = new StringBuilder();
			sql.Append("SELECT COUNT(1) FROM [SystemUser]");
			sql.Append(" WHERE [Id]=@Id ");
			var ret = false;
			using (var conn = GetOpenConnection())
			{
				ret = conn.ExecuteScalar<int>(sql.ToString(), new { @Id=Id }) > 0;
			}

			return ret;
		}

		/// <summary>
		/// 是否存在指定的SystemUser实体对象
		/// </summary>
		public static bool Exists(Expression<Func<SystemUser, bool>> predicate)
		{
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUser]");
            sql.Append(" WHERE " + where_str);
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString()) > 0;
            }

            return ret;
		}

		/// <summary>
        /// 添加新SystemUser记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
        /// <returns>新插入数据的id</returns>
        public static int Insert(SystemUser model, SqlConnection conn = null, SqlTransaction transaction = null)
        {
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [SystemUser]([Account], [Email], [Password], [Phone], [HeadImage], [Status], [CreatedTime])");
            sql.Append(" OUTPUT INSERTED.[Id] ");
            sql.Append("VALUES(@Account, @Email, @Password, @Phone, @HeadImage, @Status, @CreatedTime)");
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
        /// 删除指定的SystemUser数据记录
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUser] ");
            sql.Append(" WHERE [Id]=@Id ");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @Id=Id }) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 删除指定的SystemUser数据记录
        /// </summary>
        public static bool Delete(Expression<Func<SystemUser, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}
			
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUser]");
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
        /// 批量删除指定的SystemUser数据记录
        /// </summary>
        /// <param name="Ids">SystemUser实体对象的id列表</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(List<int> Ids)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUser] ");
            sql.Append(" WHERE [Id] IN @ids");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @ids = Ids}) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, bool reverse, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, SqlConnection conn, SqlTransaction transaction, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, Expression<Func<SystemUser, bool>> predicate, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, Expression<Func<SystemUser, bool>> predicate, bool reverse, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, Expression<Func<SystemUser, bool>> predicate, SqlConnection conn, SqlTransaction transaction, params SystemUserColumn[] fields)
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
        /// 更新SystemUser数据记录
        /// </summary>
        /// <param name="model">SystemUser实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUser model, Expression<Func<SystemUser, bool>> predicate, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserColumn[] fields)
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
            sql.Append("UPDATE [SystemUser]");
            if (fields == null || fields.Length == 0)
            {
                 sql.Append(" SET [Account] = @Account, [Email] = @Email, [Password] = @Password, [Phone] = @Phone, [HeadImage] = @HeadImage, [Status] = @Status");
            }
            else
            {
				if (reverse == true)
				{
					 fields = (SystemUserColumn[])Columns.All.Except(fields);
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
        /// 获取指定的SystemUser实体对象
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>SystemUser实体</returns>
        public static SystemUser GetModel(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 [Id], [Account], [Email], [Password], [Phone], [HeadImage], [Status], [CreatedTime] FROM [SystemUser] ");
            sql.Append(" WHERE [Id]=@Id ");
            SystemUser ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUser>(sql.ToString(), new { @Id=Id });
            }

            return ret;
        }

		/// <summary>
        /// 获取指定的SystemUser实体对象
        /// </summary>
        public static SystemUser GetModel(Expression<Func<SystemUser, bool>> predicate)
        {
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);
			
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 * FROM [SystemUser] ");
            sql.Append(" WHERE " + where_str);
            SystemUser ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUser>(sql.ToString());
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUser实体对象
        /// </summary>
        public static List<SystemUser> GetList(Expression<Func<SystemUser, bool>> predicate = null, params SystemUserColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT * FROM [SystemUser]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUser> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUser>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUser实体对象
        /// </summary>
        public static List<SystemUser> GetList(int top, Expression<Func<SystemUser, bool>> predicate = null, params SystemUserColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT TOP " + top.ToString() + " * FROM [SystemUser]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUser> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUser>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
		/// 获取记录总数
		/// </summary>
        public static int GetCount(Expression<Func<SystemUser, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUser]");
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
		public static PageDataView<SystemUser> GetPage(
			Expression<Func<SystemUser, bool>> predicate = null, 
			int pageSize = 20, 
			int currentPage = 1, 
			SystemUserColumn orderBy = null, 
			params SystemUserColumn[] columns)
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

			return Paged<SystemUser>(
					"[SystemUser]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public static PageDataView<SystemUser> GetPage(
			Expression<Func<SystemUser, bool>> predicate, 
			int pageSize, 
			int currentPage, 
			IList<SystemUserColumn> orderBy, 
			params SystemUserColumn[] columns)
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

			return Paged<SystemUser>(
					"[SystemUser]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }
	}
}
