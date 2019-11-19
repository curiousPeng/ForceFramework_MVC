/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// SystemRole实体
	/// 系统角色信息
	/// </summary>
	[Serializable]
	public class SystemRole
	{
		public SystemRole()
		{}

		private int _id;
		private string _name;
		private string _remark;
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
		/// 角色名称
		/// </summary>
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			set { _remark = value; }
			get { return _remark; }
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
