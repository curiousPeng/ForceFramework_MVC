using Force.Common.LightMessager.Helper;
using Force.Common.RedisTool.Helper;
using Force.DataLayer;
using Force.Model.ViewModel.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;

namespace Force.App.Controllers
{
    public class BaseController : Controller
    {
        private HttpContext _http_context;
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        private IMemoryCache _cache;
        private IRedisHelper _redis_helper;
        private IRabbitMQProducer _rabbitmq;
        private SessionUser _cache_user;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _http_context = httpContextAccessor.HttpContext;
        }
        protected IMemoryCache Cache
        {
            get
            {
                return _cache ?? (_cache = _http_context.RequestServices.GetService<IMemoryCache>());
            }
        }
        protected SessionUser CacheUser
        {
            get
            {
                return _cache_user;
            }
        }
        protected IRedisHelper RedisHlper
        {
            get
            {
                return _redis_helper ?? (_redis_helper = _http_context.RequestServices.GetService<IRedisHelper>());
            }
        }
        protected IRabbitMQProducer RabbitMQHelper
        {
            get
            {
                return _rabbitmq ?? (_rabbitmq = _http_context.RequestServices.GetService<IRabbitMQProducer>());
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取访问url地址
            var action = context.HttpContext.Request.Path.ToString().ToLower();
            if (action == "/")
            {
                action = "/home/index";
            }
            var actionModel = SystemMenuHelper.GetModel(p => p.ActionRoute == action);
            var requestMethod = context.HttpContext.Request.Method.Trim().ToLower();
            var user = context.HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(user))
            {
                if (requestMethod == "get")
                {
                    context.Result = new RedirectResult("/Login/Index");
                }
                if (requestMethod == "post")
                {
                    context.Result = new JsonResult(Util.ResponseHelper.Error("登录已失效，请重新登录"));
                }
                return;
            }
            if (actionModel == null)
            {
                if (requestMethod == "get")
                {
                    context.Result = new RedirectResult("/home/errormsg?msg=菜单还未添加，请联系管理员添加");
                }
                if (requestMethod == "post")
                {
                    context.Result = new JsonResult(Util.ResponseHelper.Error("菜单还未添加，请联系管理员添加"));
                }
                return;
            }
            _cache_user = JsonConvert.DeserializeObject<SessionUser>(user);
            //校验权限
            if (!_cache_user.UId.Equals("1") && !_cache_user.AuthMenu.Contains(actionModel.Id))
            {
                if (requestMethod == "get")
                {
                    context.Result = new RedirectResult("/home/error?errorcode=401");
                }
                if (requestMethod == "post")
                {
                    context.Result = new RedirectResult("/home/errormsg?msg=您没有权限访问此页面！");
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
