using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Force.App.Extension;
using Force.App.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Force.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            services.AddNLog();
            services.AddMvc(options=> 
            {
                options.Filters.Add<ForceActionFilter>();
            })
               .AddJsonOptions(options =>
               {
                   //设置时间格式
                   options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
               })
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRedis(Configuration.GetSection("RedisConn").Value);
            services.AddRabbitMQ(Common.LightMessager.DAL.DataBaseEnum.SqlServer, Configuration.GetSection("RabbitMqConn").Value);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //Session服务
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
           
            app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromHours(2) });
            app.UseStaticFiles();
            app.UseCookiePolicy();
            loggerFactory.AddNLog();
            env.ConfigureNLog("NLog.config");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "areaRoute",
                   template: "{area}/{controller}/{action}/{id?}"

                   );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
