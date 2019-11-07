using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.RedisTool.Base
{
    public interface IRedisBase
    {
        IDatabase DB();
        T DoWithLock<T>(string lockName, Func<T> func, bool retry = false);
        void DoWithLock(string lockName, Action action, bool retry = false);
        string GetKeyStr(string prefix, string str, bool isGlobal = false);
        string GetKeyStr(string prefix, string str1, string str2, bool isGlobal = false);
        string GetKeyStr(string prefix, string str1, string str2, string str3, bool isGlobal = false);
        string GetKeyStr(string prefix, string str1, string str2, string str3, string str4, bool isGlobal = false);
    }
}
