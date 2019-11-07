using System;
using System.Collections.Generic;
using System.Text;
using Force.Common.LightMessager.DAL.Model;

namespace Force.Common.LightMessager.DAL.Oracle
{
    /// <summary>
    /// Oracle 跟其他俩数据库差异性比较大，还得转换字段大小写，后期再补
    /// </summary>
    public class MessageQueueHelper : IMessageQueueHelper
    {
        public MessageQueue GetModelBy(long msgHash)
        {
            throw new NotImplementedException();
        }

        public int Insert(MessageQueue model)
        {
            throw new NotImplementedException();
        }

        public bool Update(long msgHash, short fromStatus, short toStatus)
        {
            throw new NotImplementedException();
        }

        public bool Update(long msgHash, short fromStatus1, short fromStatus2, short toStatus)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCanbeRemoveIsFalse(long msgHash)
        {
            throw new NotImplementedException();
        }
    }
}
