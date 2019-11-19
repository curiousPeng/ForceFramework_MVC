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
		public sealed class SystemUserRoleMappingColumn : IColumn
		{
			internal SystemUserRoleMappingColumn(string table, string name)
			{
				Table = table;
				Name = name;
			}

			public string Name { private set; get; }

			public string Table { private set; get; }

			public bool IsAddEqual { private set; get; }

			private bool _asc;
			public string Asc { get { return this._asc ? "ASC" : "DESC"; } }

			public SystemUserRoleMappingColumn SetAddEqual() { IsAddEqual ^= true; return this; }

			public SystemUserRoleMappingColumn SetOrderByAsc() { this._asc = true; return this; }

			public SystemUserRoleMappingColumn SetOrderByDesc() { this._asc = false; return this; }
		}

		public sealed class SystemUserRoleMappingTable
		{
			internal SystemUserRoleMappingTable(string name)
			{
				Name = name;
			}

			public string Name { private set; get; }
		}
	}

	public partial class SystemUserRoleMappingHelper : BaseTableHelper
	{
		public static readonly SystemUserRoleMappingTable Table = new SystemUserRoleMappingTable("SystemUserRoleMapping");

		public sealed class Columns
		{
			public static readonly SystemUserRoleMappingColumn Id = new SystemUserRoleMappingColumn("SystemUserRoleMapping", "Id");
			public static readonly SystemUserRoleMappingColumn SystemUserId = new SystemUserRoleMappingColumn("SystemUserRoleMapping", "SystemUserId");
			public static readonly SystemUserRoleMappingColumn RoleId = new SystemUserRoleMappingColumn("SystemUserRoleMapping", "RoleId");
			public static readonly SystemUserRoleMappingColumn CreatedTime = new SystemUserRoleMappingColumn("SystemUserRoleMapping", "CreatedTime");
			public static readonly List<SystemUserRoleMappingColumn> All = new List<SystemUserRoleMappingColumn> { Id, SystemUserId, RoleId, CreatedTime };
		}

		/// <summary>
		/// 是否存在指定的SystemUserRoleMapping实体对象
		/// </summary>
		/// <param name="Id">Id</param>
		/// <returns>是否存在，true为存在</returns>
		public static bool Exists(int Id)
		{
			var sql = new StringBuilder();
			sql.Append("SELECT COUNT(1) FROM [SystemUserRoleMapping]");
			sql.Append(" WHERE [Id]=@Id ");
			var ret = false;
			using (var conn = GetOpenConnection())
			{
				ret = conn.ExecuteScalar<int>(sql.ToString(), new { @Id=Id }) > 0;
			}

			return ret;
		}

		/// <summary>
		/// 是否存在指定的SystemUserRoleMapping实体对象
		/// </summary>
		public static bool Exists(Expression<Func<SystemUserRoleMapping, bool>> predicate)
		{
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUserRoleMapping]");
            sql.Append(" WHERE " + where_str);
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString()) > 0;
            }

            return ret;
		}

		/// <summary>
        /// 添加新SystemUserRoleMapping记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
        /// <returns>新插入数据的id</returns>
        public static int Insert(SystemUserRoleMapping model, SqlConnection conn = null, SqlTransaction transaction = null)
        {
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [SystemUserRoleMapping]([SystemUserId], [RoleId], [CreatedTime])");
            sql.Append(" OUTPUT INSERTED.[Id] ");
            sql.Append("VALUES(@SystemUserId, @RoleId, @CreatedTime)");
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
        /// 删除指定的SystemUserRoleMapping数据记录
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserRoleMapping] ");
            sql.Append(" WHERE [Id]=@Id ");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @Id=Id }) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 删除指定的SystemUserRoleMapping数据记录
        /// </summary>
        public static bool Delete(Expression<Func<SystemUserRoleMapping, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}
			
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserRoleMapping]");
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
        /// 批量删除指定的SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="Ids">SystemUserRoleMapping实体对象的id列表</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(List<int> Ids)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [SystemUserRoleMapping] ");
            sql.Append(" WHERE [Id] IN @ids");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @ids = Ids}) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, bool reverse, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, SqlConnection conn, SqlTransaction transaction, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, Expression<Func<SystemUserRoleMapping, bool>> predicate, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, Expression<Func<SystemUserRoleMapping, bool>> predicate, bool reverse, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, Expression<Func<SystemUserRoleMapping, bool>> predicate, SqlConnection conn, SqlTransaction transaction, params SystemUserRoleMappingColumn[] fields)
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
        /// 更新SystemUserRoleMapping数据记录
        /// </summary>
        /// <param name="model">SystemUserRoleMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(SystemUserRoleMapping model, Expression<Func<SystemUserRoleMapping, bool>> predicate, bool reverse, SqlConnection conn, SqlTransaction transaction, params SystemUserRoleMappingColumn[] fields)
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
            sql.Append("UPDATE [SystemUserRoleMapping]");
            if (fields == null || fields.Length == 0)
            {
                 sql.Append(" SET [SystemUserId] = @SystemUserId, [RoleId] = @RoleId, [CreatedTime] = @CreatedTime");
            }
            else
            {
				if (reverse == true)
				{
					 fields = (SystemUserRoleMappingColumn[])Columns.All.Except(fields);
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
        /// 获取指定的SystemUserRoleMapping实体对象
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>SystemUserRoleMapping实体</returns>
        public static SystemUserRoleMapping GetModel(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 [Id], [SystemUserId], [RoleId], [CreatedTime] FROM [SystemUserRoleMapping] ");
            sql.Append(" WHERE [Id]=@Id ");
            SystemUserRoleMapping ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUserRoleMapping>(sql.ToString(), new { @Id=Id });
            }

            return ret;
        }

		/// <summary>
        /// 获取指定的SystemUserRoleMapping实体对象
        /// </summary>
        public static SystemUserRoleMapping GetModel(Expression<Func<SystemUserRoleMapping, bool>> predicate)
        {
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);
			
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 * FROM [SystemUserRoleMapping] ");
            sql.Append(" WHERE " + where_str);
            SystemUserRoleMapping ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemUserRoleMapping>(sql.ToString());
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUserRoleMapping实体对象
        /// </summary>
        public static List<SystemUserRoleMapping> GetList(Expression<Func<SystemUserRoleMapping, bool>> predicate = null, params SystemUserRoleMappingColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT * FROM [SystemUserRoleMapping]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUserRoleMapping> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUserRoleMapping>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取SystemUserRoleMapping实体对象
        /// </summary>
        public static List<SystemUserRoleMapping> GetList(int top, Expression<Func<SystemUserRoleMapping, bool>> predicate = null, params SystemUserRoleMappingColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT TOP " + top.ToString() + " * FROM [SystemUserRoleMapping]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<SystemUserRoleMapping> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<SystemUserRoleMapping>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
		/// 获取记录总数
		/// </summary>
        public static int GetCount(Expression<Func<SystemUserRoleMapping, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [SystemUserRoleMapping]");
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
		public static PageDataView<SystemUserRoleMapping> GetPage(
			Expression<Func<SystemUserRoleMapping, bool>> predicate = null, 
			int pageSize = 20, 
			int currentPage = 1, 
			SystemUserRoleMappingColumn orderBy = null, 
			params SystemUserRoleMappingColumn[] columns)
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

			return Paged<SystemUserRoleMapping>(
					"[SystemUserRoleMapping]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public static PageDataView<SystemUserRoleMapping> GetPage(
			Expression<Func<SystemUserRoleMapping, bool>> predicate, 
			int pageSize, 
			int currentPage, 
			IList<SystemUserRoleMappingColumn> orderBy, 
			params SystemUserRoleMappingColumn[] columns)
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

			return Paged<SystemUserRoleMapping>(
					"[SystemUserRoleMapping]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }
	}
}
