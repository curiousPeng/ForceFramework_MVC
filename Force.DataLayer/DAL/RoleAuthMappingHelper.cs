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
		public sealed class RoleAuthMappingColumn : IColumn
		{
			internal RoleAuthMappingColumn(string table, string name)
			{
				Table = table;
				Name = name;
			}

			public string Name { private set; get; }

			public string Table { private set; get; }

			public bool IsAddEqual { private set; get; }

			private bool _asc;
			public string Asc { get { return this._asc ? "ASC" : "DESC"; } }

			public RoleAuthMappingColumn SetAddEqual() { IsAddEqual ^= true; return this; }

			public RoleAuthMappingColumn SetOrderByAsc() { this._asc = true; return this; }

			public RoleAuthMappingColumn SetOrderByDesc() { this._asc = false; return this; }
		}

		public sealed class RoleAuthMappingTable
		{
			internal RoleAuthMappingTable(string name)
			{
				Name = name;
			}

			public string Name { private set; get; }
		}
	}

	public partial class RoleAuthMappingHelper : BaseTableHelper
	{
		public static readonly RoleAuthMappingTable Table = new RoleAuthMappingTable("RoleAuthMapping");

		public sealed class Columns
		{
			public static readonly RoleAuthMappingColumn Id = new RoleAuthMappingColumn("RoleAuthMapping", "Id");
			public static readonly RoleAuthMappingColumn RoleId = new RoleAuthMappingColumn("RoleAuthMapping", "RoleId");
			public static readonly RoleAuthMappingColumn MenuId = new RoleAuthMappingColumn("RoleAuthMapping", "MenuId");
			public static readonly RoleAuthMappingColumn CreatedTime = new RoleAuthMappingColumn("RoleAuthMapping", "CreatedTime");
			public static readonly List<RoleAuthMappingColumn> All = new List<RoleAuthMappingColumn> { Id, RoleId, MenuId, CreatedTime };
		}

		/// <summary>
		/// 是否存在指定的RoleAuthMapping实体对象
		/// </summary>
		/// <param name="Id">Id</param>
		/// <returns>是否存在，true为存在</returns>
		public static bool Exists(int Id)
		{
			var sql = new StringBuilder();
			sql.Append("SELECT COUNT(1) FROM [RoleAuthMapping]");
			sql.Append(" WHERE [Id]=@Id ");
			var ret = false;
			using (var conn = GetOpenConnection())
			{
				ret = conn.ExecuteScalar<int>(sql.ToString(), new { @Id=Id }) > 0;
			}

			return ret;
		}

		/// <summary>
		/// 是否存在指定的RoleAuthMapping实体对象
		/// </summary>
		public static bool Exists(Expression<Func<RoleAuthMapping, bool>> predicate)
		{
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [RoleAuthMapping]");
            sql.Append(" WHERE " + where_str);
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.ExecuteScalar<int>(sql.ToString()) > 0;
            }

            return ret;
		}

		/// <summary>
        /// 添加新RoleAuthMapping记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
        /// <returns>新插入数据的id</returns>
        public static int Insert(RoleAuthMapping model, SqlConnection conn = null, SqlTransaction transaction = null)
        {
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [RoleAuthMapping]([RoleId], [MenuId], [CreatedTime])");
            sql.Append(" OUTPUT INSERTED.[Id] ");
            sql.Append("VALUES(@RoleId, @MenuId, @CreatedTime)");
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
        /// 删除指定的RoleAuthMapping数据记录
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [RoleAuthMapping] ");
            sql.Append(" WHERE [Id]=@Id ");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @Id=Id }) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 删除指定的RoleAuthMapping数据记录
        /// </summary>
        public static bool Delete(Expression<Func<RoleAuthMapping, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}
			
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [RoleAuthMapping]");
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
        /// 批量删除指定的RoleAuthMapping数据记录
        /// </summary>
        /// <param name="Ids">RoleAuthMapping实体对象的id列表</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Delete(List<int> Ids)
        {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM [RoleAuthMapping] ");
            sql.Append(" WHERE [Id] IN @ids");
            var ret = false;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Execute(sql.ToString(), new { @ids = Ids}) > 0;
            }

            return ret;
        }

		/// <summary>
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, bool reverse, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, SqlConnection conn, SqlTransaction transaction, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, bool reverse, SqlConnection conn, SqlTransaction transaction, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, Expression<Func<RoleAuthMapping, bool>> predicate, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, Expression<Func<RoleAuthMapping, bool>> predicate, bool reverse, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, Expression<Func<RoleAuthMapping, bool>> predicate, SqlConnection conn, SqlTransaction transaction, params RoleAuthMappingColumn[] fields)
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
        /// 更新RoleAuthMapping数据记录
        /// </summary>
        /// <param name="model">RoleAuthMapping实体对象</param>
		/// <param name="fields">需要更新的字段名字</param>
		/// <param name="reverse">反转给定的更新字段</param>
        /// <returns>是否成功，true为成功</returns>
        public static bool Update(RoleAuthMapping model, Expression<Func<RoleAuthMapping, bool>> predicate, bool reverse, SqlConnection conn, SqlTransaction transaction, params RoleAuthMappingColumn[] fields)
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
            sql.Append("UPDATE [RoleAuthMapping]");
            if (fields == null || fields.Length == 0)
            {
                 sql.Append(" SET [RoleId] = @RoleId, [MenuId] = @MenuId, [CreatedTime] = @CreatedTime");
            }
            else
            {
				if (reverse == true)
				{
					 fields = (RoleAuthMappingColumn[])Columns.All.Except(fields);
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
        /// 获取指定的RoleAuthMapping实体对象
        /// </summary>
		/// <param name="Id">Id</param>
        /// <returns>RoleAuthMapping实体</returns>
        public static RoleAuthMapping GetModel(int Id)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 [Id], [RoleId], [MenuId], [CreatedTime] FROM [RoleAuthMapping] ");
            sql.Append(" WHERE [Id]=@Id ");
            RoleAuthMapping ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<RoleAuthMapping>(sql.ToString(), new { @Id=Id });
            }

            return ret;
        }

		/// <summary>
        /// 获取指定的RoleAuthMapping实体对象
        /// </summary>
        public static RoleAuthMapping GetModel(Expression<Func<RoleAuthMapping, bool>> predicate)
        {
			var parser = new PredicateParser();
            var where_str = parser.Parse(predicate);
			
            var sql = new StringBuilder();
            sql.Append("SELECT TOP 1 * FROM [RoleAuthMapping] ");
            sql.Append(" WHERE " + where_str);
            RoleAuthMapping ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<RoleAuthMapping>(sql.ToString());
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取RoleAuthMapping实体对象
        /// </summary>
        public static List<RoleAuthMapping> GetList(Expression<Func<RoleAuthMapping, bool>> predicate = null, params RoleAuthMappingColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT * FROM [RoleAuthMapping]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<RoleAuthMapping> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<RoleAuthMapping>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
        /// 批量获取RoleAuthMapping实体对象
        /// </summary>
        public static List<RoleAuthMapping> GetList(int top, Expression<Func<RoleAuthMapping, bool>> predicate = null, params RoleAuthMappingColumn[] orderBy)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT TOP " + top.ToString() + " * FROM [RoleAuthMapping]");
            if (where_str != string.Empty)
				sql.Append(" WHERE " + where_str);
			if (orderBy != null && orderBy.Length > 0)
                sql.Append(" ORDER BY " + string.Join(", ", orderBy.Select(p => $"{p.Name} {p.Asc}")));

			List<RoleAuthMapping> ret = null;
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<RoleAuthMapping>(sql.ToString()).ToList();
            }
			
            return ret;
        }

		/// <summary>
		/// 获取记录总数
		/// </summary>
        public static int GetCount(Expression<Func<RoleAuthMapping, bool>> predicate = null)
        {
			var where_str = string.Empty;
			if (predicate != null)
			{
				var parser = new PredicateParser();
				where_str = parser.Parse(predicate);
			}

            var sql = new StringBuilder();
            sql.Append("SELECT COUNT(1) FROM [RoleAuthMapping]");
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
		public static PageDataView<RoleAuthMapping> GetPage(
			Expression<Func<RoleAuthMapping, bool>> predicate = null, 
			int pageSize = 20, 
			int currentPage = 1, 
			RoleAuthMappingColumn orderBy = null, 
			params RoleAuthMappingColumn[] columns)
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

			return Paged<RoleAuthMapping>(
					"[RoleAuthMapping]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public static PageDataView<RoleAuthMapping> GetPage(
			Expression<Func<RoleAuthMapping, bool>> predicate, 
			int pageSize, 
			int currentPage, 
			IList<RoleAuthMappingColumn> orderBy, 
			params RoleAuthMappingColumn[] columns)
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

			return Paged<RoleAuthMapping>(
					"[RoleAuthMapping]", 
					where_str, 
					orderby_str, 
					column_str, 
					pageSize, 
					currentPage);
        }
	}
}
