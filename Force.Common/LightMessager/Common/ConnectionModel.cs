using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.LightMessager.Common
{
    public class ConnectionModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// 启用自动恢复
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }
        /// <summary>
        /// 网络恢复间隔，秒
        /// </summary>
        public TimeSpan NetworkRecoveryInterval { get; set; }
    }
}