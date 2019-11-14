using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Force.App.Util
{
    public class ConfigurationManager
    {
        private static IConfigurationRoot configurationRoot;
        static ConfigurationManager()
        {
            // 添加json配置文件路径
#if LOCAL
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.Local.json");
#elif DEBUG
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.Development.json");
#else
            var builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json");
#endif
            // 创建配置根对象
           configurationRoot = builder.Build();
        }
        private static string GetResourceApi(string name)
        {
         
            return configurationRoot.GetSection(name).Value;
        }
        public static NameValueCollection AppSettings
        {
            get { return new NameValueCollection { ["ResourceApi"] = GetResourceApi("ResourceInfo:ResourceApi"),["Version"]= GetResourceApi("ResourceInfo:Version"), ["Environment"] = GetResourceApi("ResourceInfo:Environment") }; }
        }
    }
}
