using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Force.App.Filter
{
    public class ForceExceptionFilter : IExceptionFilter
    {
        protected static Logger log = LogManager.GetCurrentClassLogger();
        readonly IWebHostEnvironment _env;//环境变量
        public ForceExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (_env.IsDevelopment())
            {
                log.Error(context.Exception.ToString());
                if (context.HttpContext.Request.Method.ToLower() == "post")
                {
                    context.Result = new JsonResult(Util.ResponseHelper.Error("出现内部错误!请稍后再试!"));
                }
                return;
            }
            else
            {
                log.Error(context.Exception.ToString());
                context.ExceptionHandled = true;
                context.Result = new RedirectResult("/home/error");
                return;
            }
        }
    }
}
