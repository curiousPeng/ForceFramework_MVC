/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// RoleAuthMapping实体
	/// 角色跟对应权限项之间的关系
	/// </summary>
	[Serializable]
	public class RoleAuthMapping
	{
		public RoleAuthMapping()
		{}

		private int _id;
		private int _roleid;
		private int _menuid;
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
		/// 角色Id
		/// </summary>
		public int RoleId
		{
			set { _roleid = value; }
			get { return _roleid; }
		}

		/// <summary>
		/// 菜单Id
		/// </summary>
		public int MenuId
		{
			set { _menuid = value; }
			get { return _menuid; }
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
