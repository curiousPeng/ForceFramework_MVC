using Force.Common.RedisTool.Base;
using Force.Common.RedisTool.Helper;
using Force.Common.LightMessager.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Common.LightMessager.Helper;

namespace Force.App.Extension
{
    public static class RabbitMQExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="connString">数据库连接字符串</param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, DataBaseEnum type, string connString)
        {
            switch (type)
            {
                case DataBaseEnum.Mysql:
                    services.AddSingleton(typeof(IMessageQueueHelper), _ => new Common.LightMessager.DAL.Mysql.MessageQueueHelper(connString));
                    break;
                case DataBaseEnum.SqlServer:
                    services.AddSingleton(typeof(IMessageQueueHelper), _ => new Common.LightMessager.DAL.SqlServer.MessageQueueHelper(connString));
                    break;
                case DataBaseEnum.Oracle:
                default:
                    throw new NotSupportedException();

            }

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            return services;
        }
    }
}
