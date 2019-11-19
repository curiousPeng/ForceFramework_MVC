/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// SystemUser实体
	/// 包含后台用户数据
	/// </summary>
	[Serializable]
	public class SystemUser
	{
		public SystemUser()
		{}

		private int _id;
		private string _account;
		private string _nickname;
		private string _email;
		private string _password;
		private string _phone;
		private string _headimage;
		private short _status;
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
		/// 账户
		/// </summary>
		public string Account
		{
			set { _account = value; }
			get { return _account; }
		}

		/// <summary>
		/// 昵称
		/// </summary>
		public string NickName
		{
			set { _nickname = value; }
			get { return _nickname; }
		}

		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email
		{
			set { _email = value; }
			get { return _email; }
		}

		/// <summary>
		/// 密码
		/// </summary>
		public string Password
		{
			set { _password = value; }
			get { return _password; }
		}

		/// <summary>
		/// 手机
		/// </summary>
		public string Phone
		{
			set { _phone = value; }
			get { return _phone; }
		}

		/// <summary>
		/// 头像
		/// </summary>
		public string HeadImage
		{
			set { _headimage = value; }
			get { return _headimage; }
		}

		/// <summary>
		/// 状态[1 正常 2 冻结]
		/// </summary>
		public short Status
		{
			set { _status = value; }
			get { return _status; }
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
