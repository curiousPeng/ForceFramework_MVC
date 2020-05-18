using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Force.App.Util;
using Force.Common.AES;
using Force.DataLayer;
using Force.Model;
using Force.Model.ViewModel.SystemUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Force.App.Controllers
{
    public class SystemUserController : BaseController
    {
        private IConfiguration _configuration;
        public SystemUserController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
        }

        // GET: SystemUser
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([FromForm] UserPageReq ReqModel)
        {
            //当前页数
            var nowpage = (ReqModel.start + ReqModel.length) / ReqModel.length;

            Expression<Func<SystemUser, bool>> exp = p => p.Id > 0;
            if (!string.IsNullOrEmpty(ReqModel.Name))
            {
                exp = p => p.Email.Contains(ReqModel.Name) || p.Account.Contains(ReqModel.Name) || p.Phone.Contains(ReqModel.Name);
            }

            var dataList = SystemUserHelper.GetPage(exp, pageSize: ReqModel.length, currentPage: nowpage);
            long total = dataList.TotalPages;
            var itemList = dataList.Items;

            var totalPage = Math.Ceiling(Convert.ToDouble(total / ReqModel.length));
            var model = new List<SystemUserRes>();
            foreach (var item in itemList)
            {
                model.Add(new SystemUserRes()
                {
                    UseCode = item.Id,
                    CreatedTime = item.CreatedTime,
                    Status = item.Status,
                    Name = item.NickName,
                    Account = item.Account,
                    Email = item.Email,
                    HeadImg = string.IsNullOrEmpty(item.HeadImage) ? _configuration.GetSection("ResourceInfo:ResourceApi").Value + "/assets/layouts/layout/img/avatar3_small.jpg" : _configuration.GetSection("ImgURL").Value + item.HeadImage,
                    Phone = item.Phone
                });
            }
            return Json(new { data = model, draw = ReqModel.draw, recordsTotal = total, recordsFiltered = total });
        }

        // GET: SystemUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SystemUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] SystemUserCreate model)
        {
            if (SystemUserHelper.Exists(p => p.NickName == model.Name || p.Phone == model.Phone || p.Email == model.Email))
            {
                return Json(ResponseHelper.Error("该用户已经存在！"));
            }
            var UserModel = new SystemUser
            {
                Account = model.Account,
                CreatedTime = DateTime.Now,
                Email = model.Email,
                HeadImage = "",
                NickName = model.Name,
                Password = AESUtil.Md5(model.Pwd),
                Phone = model.Phone,
                Status = model.IsUse
            };

            SystemUserHelper.Insert(UserModel);
            return Json(ResponseHelper.Success("ok"));

        }

        // GET: SystemUser/Edit
        [HttpGet]
        public ActionResult Edit()
        {
            var useCode = Request.Query["code"];
            if (string.IsNullOrEmpty(useCode))
            {
                return Redirect("/home/error");
            }
            var data = SystemUserHelper.GetModel(int.Parse(useCode));
            if (data == null)
            {
               return new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("用户不存在"));
            }
            return View(data);
        }

        // POST: SystemUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] SystemUserEdit model)
        {
            if (model.Id == 1)
            {
                return Json(ResponseHelper.Error("该用户不可被编辑"));
            }
            if (!string.IsNullOrEmpty(model.Pwd))
            {
                if (model.Pwd.Length < 6 || model.Pwd.Length > 16)
                {
                    return Json(ResponseHelper.Error("密码长度不能少于6位大于16位！"));
                }
            }
            var UserModel = SystemUserHelper.GetModel(model.Id);
            //查询是否存重名
            if (SystemUserHelper.Exists(p => p.Id != model.Id && (p.Phone.Equals(model.Phone) || p.Email.Equals(model.Email))))
            {
                return Json(new { status = 0, msg = "已存在相同的手机号或者邮箱,请修改！" });
            }
            UserModel.Email = model.Email;

            UserModel.Status = model.IsUse;
            UserModel.NickName = model.Name;
            UserModel.Phone = model.Phone;
            if (!string.IsNullOrEmpty(model.Pwd))
            {
                UserModel.Password = AESUtil.Md5(model.Pwd);
            }
            SystemUserHelper.Update(UserModel);
            return Json(ResponseHelper.Success("ok"));

        }

        // Post: SystemUser/ChangeStatus
        [HttpPost]
        public ActionResult ChangeStatus()
        {
            var status = Convert.ToInt16(Request.Form["status"]);
            var code = int.Parse(Request.Form["id"]);

            SystemUserHelper.Update(new SystemUser { Id = code, Status = status }, SystemUserHelper.Columns.Status);
            return new JsonResult(ResponseHelper.Success("ok"));
        }

        [HttpGet]
        public ActionResult UserRole(int user)
        {
            var userModel = SystemUserRoleMappingHelper.GetUserRoleBy(user);

            if (userModel == null)
            {
              return  new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("用户不存在"));
            }
            var roleModel = SystemRoleHelper.GetList(p => p.Id != 1);
            ViewBag.Role = roleModel;
            return View(userModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRole(int userId,int roleId)
        {
            var userModel = SystemUserRoleMappingHelper.GetUserRoleBy(userId);
            if (userModel == null)
            {
                return new JsonResult(ResponseHelper.Error("用户不存在"));
            }
            if (roleId == userModel.RoleId)
            {
                return new JsonResult(ResponseHelper.Success("ok"));
            }
            var roleModel = SystemRoleHelper.GetModel(p => p.Id == roleId);
            if (roleModel == null)
            {
                return new JsonResult(ResponseHelper.Error("该角色不存在！"));
            }
            var userRoleMapping = SystemUserRoleMappingHelper.GetModel(p => p.SystemUserId == userId);
            if (userRoleMapping == null)
            {
                userRoleMapping = new SystemUserRoleMapping { CreatedTime = DateTime.Now, RoleId = roleId, SystemUserId = userId };
                SystemUserRoleMappingHelper.Insert(userRoleMapping);
                return new JsonResult(ResponseHelper.Success("ok"));
            }
            if(SystemUserRoleMappingHelper.Update(userRoleMapping, p => p.SystemUserId == userId, SystemUserRoleMappingHelper.Columns.RoleId))
            {
                return new JsonResult(ResponseHelper.Success("ok"));
            }
            return new JsonResult(ResponseHelper.Error("修改失败！"));
        }
    }
}