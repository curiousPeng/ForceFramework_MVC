using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Force.App.Extension
{
    public static class NLogServiceExtension
    {
        public static IServiceCollection AddNLog(this IServiceCollection services)
        {
            services.RemoveAll<ILoggerProvider>();

            services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                return new NLogLoggerProvider(NLogAspNetCoreOptions.Default);
            });

            return services;
        }
    }
}
