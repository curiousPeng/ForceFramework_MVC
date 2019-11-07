using System;

namespace Force.Common.LightMessager.DAL.Model
{
    /// <summary>
    /// LightMessager专用消息落地表实体
    /// </summary>
    [Serializable]
    public class MessageQueue
	{
		public MessageQueue()
		{}
		private int _id;
		private long _msghash;
		private string _msgcontent;
		private short _status;
		private int _retrycount;
		private DateTime? _lastretrytime;
		private bool _canberemoved;
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
		/// MsgHash
		/// </summary>
		public long MsgHash
		{
			set { _msghash = value; }
			get { return _msghash; }
		}

		/// <summary>
		/// 消息内容
		/// </summary>
		public string MsgContent
		{
			set { _msgcontent = value; }
			get { return _msgcontent; }
		}

		/// <summary>
		/// 状态[1 Created 2 Retrying 3 ArrivedBroker 4 ArrivedConsumer 5 Exception 6 Processed]
		/// </summary>
		public short Status
		{
			set { _status = value; }
			get { return _status; }
		}

		/// <summary>
		/// 重试次数
		/// </summary>
		public int RetryCount
		{
			set { _retrycount = value; }
			get { return _retrycount; }
		}

		/// <summary>
		/// 最近重试时间
		/// </summary>
		public DateTime? LastRetryTime
		{
			set { _lastretrytime = value; }
			get { return _lastretrytime; }
		}

		/// <summary>
		/// 能否被删除
		/// </summary>
		public bool CanBeRemoved
		{
			set { _canberemoved = value; }
			get { return _canberemoved; }
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
