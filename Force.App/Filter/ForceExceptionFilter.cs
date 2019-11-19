using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Model.ViewModel.Response;
using Microsoft.AspNetCore.Mvc;
using Force.Model.ViewModel.SubHeader;
using Microsoft.AspNetCore.Http;
using Force.DataLayer;
using Microsoft.AspNetCore.Hosting;
using NLog;

namespace Force.App.Filter
{
    public class ForceExceptionFilter : IExceptionFilter
    {
        protected static Logger log = LogManager.GetCurrentClassLogger();
        readonly IHostingEnvironment _env;//环境变量
        public ForceExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (_env.IsDevelopment())
            {
                log.Error(context.Exception.ToString());
                context.Result = new RedirectResult("/home/error?ErrorCode="+context.HttpContext.Response.StatusCode);
            }
            else
            {
                log.Error(context.Exception.ToString());
                context.ExceptionHandled = true;
                context.Result = new RedirectResult("/home/error");
            }
        }
    }
}
