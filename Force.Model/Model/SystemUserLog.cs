/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// SystemUserLog实体
	/// 用户操作日志
	/// </summary>
	[Serializable]
	public class SystemUserLog
	{
		public SystemUserLog()
		{}

		private int _id;
		private int _systemuserid;
		private string _systemusername;
		private string _actionroute;
		private string _details;
		private short _type;
		private string _ip;
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
		/// 用户id
		/// </summary>
		public int SystemUserId
		{
			set { _systemuserid = value; }
			get { return _systemuserid; }
		}

		/// <summary>
		/// 用户名称
		/// </summary>
		public string SystemUserName
		{
			set { _systemusername = value; }
			get { return _systemusername; }
		}

		/// <summary>
		/// Action 路由
		/// </summary>
		public string ActionRoute
		{
			set { _actionroute = value; }
			get { return _actionroute; }
		}

		/// <summary>
		/// 操作详情
		/// </summary>
		public string Details
		{
			set { _details = value; }
			get { return _details; }
		}

		/// <summary>
		/// 类型 [1 后台登录退出 2 菜单操作 3 角色操作 4 操作用户 5 后台其他]
		/// </summary>
		public short Type
		{
			set { _type = value; }
			get { return _type; }
		}

		/// <summary>
		/// IP
		/// </summary>
		public string IP
		{
			set { _ip = value; }
			get { return _ip; }
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
