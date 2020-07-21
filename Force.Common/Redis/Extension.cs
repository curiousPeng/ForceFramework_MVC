using StackExchange.Redis;
using System;

namespace Force.Common.RedisTools
{
    internal static class _Extension
    {
        public static T As<T>(this RedisValue value)
        {
            // link: https://stackoverflow.com/questions/8171412/cannot-implicitly-convert-type-int-to-t
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
