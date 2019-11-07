using Force.Common.RedisTool.Base;
using Force.Common.RedisTool.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Force.App.Extension
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,string redisConnString)
        {
            services.AddSingleton(typeof(IRedisBase), _ => new RedisBase(redisConnString));

            services.AddSingleton<IRedisHelper,RedisHelper>();

            return services;
        }
    }
}
