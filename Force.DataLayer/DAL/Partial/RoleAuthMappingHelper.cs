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
using Force.Model.ViewModel.SystemRole;

namespace Force.DataLayer
{
    public partial class RoleAuthMappingHelper
    {
        /// <summary>
        /// 根据角色ID获取菜单
        /// </summary>
        /// <param name="RoleId">角色id</param>
        /// <returns></returns>
        public static List<RoleAuthMenu> GetRoleAuthMenuBy(int RoleId)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT a.Id,a.RoleId,a.MenuId,b.Name,b.ParentId,b.Icon FROM RoleAuthMapping a,SystemMenu b WHERE a.MenuId= b.Id AND a.RoleId=@Id");
            var ret = new List<RoleAuthMenu>();
            using (var conn = GetOpenConnection())
            {
                ret = conn.Query<RoleAuthMenu>(sql.ToString(), new { @Id = RoleId }).ToList();
            }

            return ret;
        }
    }
}
