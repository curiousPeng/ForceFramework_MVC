/*
 *  2019-11-18 11:10:22
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;
using System.Collections.Generic;

namespace Force.Model
{
	/// <summary>
	/// UploadFileInfo实体
	/// 用于记录上传的文件（可能是图片、apk包等等）
	/// </summary>
	[Serializable]
	public class UploadFileInfo
	{
		public UploadFileInfo()
		{}

		private int _id;
		private string _name;
		private string _url;
		private short _source;
		private string _type;
		private bool _isuse;
		private string _hashval;
		private int _physicalpath;
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
		/// 名称
		/// </summary>
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}

		/// <summary>
		/// URL
		/// </summary>
		public string URL
		{
			set { _url = value; }
			get { return _url; }
		}

		/// <summary>
		/// 来源[1 admin后台 2 agent代理系统 3 商户系统]
		/// </summary>
		public short Source
		{
			set { _source = value; }
			get { return _source; }
		}

		/// <summary>
		/// 文件类型
		/// </summary>
		public string Type
		{
			set { _type = value; }
			get { return _type; }
		}

		/// <summary>
		/// 是否在使用
		/// </summary>
		public bool IsUse
		{
			set { _isuse = value; }
			get { return _isuse; }
		}

		/// <summary>
		/// Hash值
		/// </summary>
		public string HashVal
		{
			set { _hashval = value; }
			get { return _hashval; }
		}

		/// <summary>
		/// 物理路径
		/// </summary>
		public int PhysicalPath
		{
			set { _physicalpath = value; }
			get { return _physicalpath; }
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
