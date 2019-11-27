using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Force.App.Util;
using Force.DataLayer;
using Force.DataLayer.Metadata;
using Force.Model;
using Force.Model.ViewModel.SystemRole;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class SystemRoleController : BaseController
    {
        public SystemRoleController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        // GET: SystemRole
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([FromForm] SystemRolePageReq ReqModel)
        {
            //当前页数
            var nowpage = (ReqModel.start + ReqModel.length) / ReqModel.length;

            Expression<Func<SystemRole, bool>> exp = p => p.Id > 0;
            if (!string.IsNullOrEmpty(ReqModel.Name))
            {
                exp = p => p.Name.Contains(ReqModel.Name);
            }

            var dataList = SystemRoleHelper.GetPage(exp, pageSize: ReqModel.length, currentPage: nowpage);
            long total = dataList.TotalPages;
            var itemList = dataList.Items;

            var totalPage = Math.Ceiling(Convert.ToDouble(total / ReqModel.length));
            var model = new List<SystemRolePageList>();
            foreach (var item in itemList)
            {
                model.Add(new SystemRolePageList()
                {
                    RoleId = item.Id,
                    CreatedTime = item.CreatedTime,
                    RoleName = item.Name,
                    RoleRemark = item.Remark
                });
            }
            return Json(new { data = model, draw = ReqModel.draw, recordsTotal = total, recordsFiltered = total });
        }

        // GET: SystemRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] SystemRoleCreate ReqModel)
        {

            // TODO: Add insert logic here
            if (SystemRoleHelper.Exists(p => p.Name.Equals(ReqModel.RoleName)))
            {
                return Json(ResponseHelper.Error("该角色名已经存在!"));
            }
            var systemRole = new SystemRole
            {
                CreatedTime = DateTime.Now,
                Name = ReqModel.RoleName,
                Remark = ReqModel.Remark
            };
            SystemRoleHelper.Insert(systemRole);
            return Json(ResponseHelper.Success("ok"));

        }

        // GET: SystemRole/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                new RedirectResult("/home/errormsg?msg="+ WebUtility.UrlEncode("错误的请求方式！"));
            }
            var role = SystemRoleHelper.GetModel(p => p.Id == id);
            if (role == null)
            {
                new RedirectResult("/home/errormsg?msg=未找到该角色");
            }
            return View(role);
        }

        // POST: SystemRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] SystemRoleEdit ReqModel)
        {
            var role = SystemRoleHelper.GetModel(p => p.Id == ReqModel.Id);
            if (role == null)
            {
                new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("未找到该角色"));
            }
            if (SystemRoleHelper.Exists(p => p.Id != role.Id && p.Name.Equals(ReqModel.RoleName)))
            {
                return Json(ResponseHelper.Error("该角色名已经存在!"));
            }
            role.Name = ReqModel.RoleName;
            role.Remark = ReqModel.Remark;
            SystemRoleHelper.Update(role, SystemRoleHelper.Columns.Name, SystemRoleHelper.Columns.Remark);
            return Json(ResponseHelper.Success("ok"));

        }

        // POST: SystemRole/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (SystemUserRoleMappingHelper.Exists(p => p.RoleId == id))
            {
                return Json(ResponseHelper.Error("有用户正在使用角色无法删除!"));
            }
            SystemRoleHelper.Delete(id);
            return Json(ResponseHelper.Success("ok"));
        }
    }
}