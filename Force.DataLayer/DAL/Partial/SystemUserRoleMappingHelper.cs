/*
 *  2019-03-12 16:13:58
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using Dapper;
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
	public partial class SystemUserRoleMappingHelper
	{
        /// <summary>
        /// 根据用户id获取角色id和名称
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>是否存在，true为存在</returns>
        public static SystemRole GetRoleBy(int userId)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT b.* FROM [SystemUserRoleMapping] a, [SystemRole] b WHERE a.RoleId=b.Id AND a.SystemUserId=@Id");
            SystemRole ret;
            using (var conn = GetOpenConnection())
            {
                ret = conn.QueryFirstOrDefault<SystemRole>(sql.ToString(), new { @Id = userId });
            }

            return ret;
        }
    }
}
