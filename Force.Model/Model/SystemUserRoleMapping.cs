/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// SystemUserRoleMapping实体
	/// 用户与所对应的角色之间的关系
	/// </summary>
	[Serializable]
	public class SystemUserRoleMapping
	{
		public SystemUserRoleMapping()
		{}

		private int _id;
		private int _systemuserid;
		private int _roleid;
		private DateTime _createdtime;


		/// <summary>
		/// Id
		/// </summary>
		public int Id
		{
			set { _id = value; }
			get { return _id; }
		}

		/// <summary>
		/// 用户Id
		/// </summary>
		public int SystemUserId
		{
			set { _systemuserid = value; }
			get { return _systemuserid; }
		}

		/// <summary>
		/// 角色Id
		/// </summary>
		public int RoleId
		{
			set { _roleid = value; }
			get { return _roleid; }
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedTime
		{
			set { _createdtime = value; }
			get { return _createdtime; }
		}


	}
}
