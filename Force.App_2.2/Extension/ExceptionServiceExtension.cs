using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Force.App.Extension
{
    public static class ExceptionalServiceExtension
    {
        public static IServiceCollection AddTheExceptional(this IServiceCollection services, IConfiguration config)
        {
            services.AddExceptional(config);
            return services;
        }
    }
}
