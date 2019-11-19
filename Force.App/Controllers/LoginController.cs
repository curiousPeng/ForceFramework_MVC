using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.App.Util;
using Force.Common.AES;
using Force.DataLayer;
using Force.GenEnum;
using Force.Model.ViewModel.Login;
using Force.Model.ViewModel.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Force.App.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login/SignIn
        [HttpPost]
        public ActionResult SignIn([FromForm] SignInModel model)
        {
            try
            {
                var UserString = HttpContext.Session.GetString("UserInfo");

                if (!string.IsNullOrEmpty(UserString))
                {
                    return Redirect("/home/index");
                }
                // TODO: Add login logic here
                var password = AESUtil.Md5(model.Password);
                var user = SystemUserHelper.GetModel(p => p.Password == password && (p.Email == model.Account || p.Account == model.Account || p.Phone == model.Account));
                if (user == null)
                {
                    return Json(ResponseHelper.Error("账户或密码错误，请确认后再试！"));
                }
                if(user.Status!= SystemUser_Status_Enum.正常)
                {
                    return Json(ResponseHelper.Error("账户已被冻结！"));
                }
                //获取用户角色
                var role = SystemUserRoleMappingHelper.GetRoleBy(user.Id);
                if (role == null)
                {
                    return Json(ResponseHelper.Error("该账户还未分配角色请联系管理员!"));
                }
                var roleAuthList= RoleAuthMappingHelper.GetList(p => p.RoleId == role.Id);
                if (roleAuthList.Count < 1)
                {
                    return Json(ResponseHelper.Error("角色未拥有权限,请联系下管理员处理"));
                }
                var token = Guid.NewGuid().ToString("N");
                //存session
                var UserCache = new SessionUser
                {
                    HeadImg = user.HeadImage,
                    Token = token,
                    UId = user.Id.ToString(),
                    UserName = user.NickName,
                    RoleId = role.Id,
                    Email = user.Email,
                    RoleName = role.Name,
                    AuthMenu = roleAuthList.Select(p=>p.MenuId).ToList()
                };
                HttpContext.Session.SetString("UserInfo", JsonConvert.SerializeObject(UserCache));
                //返回用户信息
                return Json(ResponseHelper.Success(UserCache));
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/out
        public ActionResult Out()
        {
            HttpContext.Session.Clear();
            return Redirect("/login/index");
        }
    }
}