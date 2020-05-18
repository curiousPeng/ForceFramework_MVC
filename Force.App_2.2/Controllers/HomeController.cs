using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Model.ViewModel.SubHeader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        // GET: Home
        public ActionResult Index()
        {
            var subHeader = new SubHeader { Title = "Dashboard", PageTitle= "Dashboard", SubPageTitle= "dashboard & statistics", LinkText = new Dictionary<string, string> { { "主页", "/home/index" } } };
            ViewData["SubHeader"] = subHeader;
            return View();
        }

        public ActionResult Error(int ErrorCode)
        {
            switch (ErrorCode)
            {
                case StatusCodes.Status404NotFound:
                    return View("Error404");
                case StatusCodes.Status401Unauthorized:
                case StatusCodes.Status402PaymentRequired:
                case StatusCodes.Status403Forbidden:
                    return View("Error401");
                default:
                    return View("Error");
            }
        }

        public ActionResult ErrorMsg(string msg="请求出现了错误！")
        {
            ViewBag.Msg = msg;
            return View("ErrorModal");
        }
    }
}