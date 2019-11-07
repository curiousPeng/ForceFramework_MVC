/*
 *  2019-03-12 16:13:58
 *  本文件由生成工具自动生成，请勿随意修改内容除非你很清楚自己在做什么！
 */
using System;

namespace Force.Model
{
    /// <summary>
    /// SystemMenu实体
    /// 菜单列表
    /// </summary>
    [Serializable]
	public class SystemMenu
	{
		public SystemMenu()
		{}

		private int _id;
		private string _name;
		private int _parentid;
		private string _parentlist;
		private string _actionroute;
		private string _icon;
		private short _type;
		private int _sort;
		private int _depth;
		private bool _isuse;
		private string _remark;
		private DateTime _createdtime;
		private int ___pagerow;

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
		/// 父级Id
		/// </summary>
		public int ParentId
		{
			set { _parentid = value; }
			get { return _parentid; }
		}

		/// <summary>
		/// 父级列表
		/// </summary>
		public string ParentList
		{
			set { _parentlist = value; }
			get { return _parentlist; }
		}

		/// <summary>
		/// Action路由
		/// </summary>
		public string ActionRoute
		{
			set { _actionroute = value; }
			get { return _actionroute; }
		}

		/// <summary>
		/// 图标
		/// </summary>
		public string Icon
		{
			set { _icon = value; }
			get { return _icon; }
		}

		/// <summary>
		/// 类型 [1 菜单 2 新增 3 编辑 4 删除 5 查询 6 页面]
		/// </summary>
		public short Type
		{
			set { _type = value; }
			get { return _type; }
		}

		/// <summary>
		/// 排序
		/// </summary>
		public int Sort
		{
			set { _sort = value; }
			get { return _sort; }
		}

		/// <summary>
		/// 菜单层级深度
		/// </summary>
		public int Depth
		{
			set { _depth = value; }
			get { return _depth; }
		}

		/// <summary>
		/// 是否使用
		/// </summary>
		public bool IsUse
		{
			set { _isuse = value; }
			get { return _isuse; }
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

		public int __PageRow
		{
			set { ___pagerow = value; }
			get { return ___pagerow; }
		}
	}
}
