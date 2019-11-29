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
              return  new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("错误的请求方式！"));
            }
            var role = SystemRoleHelper.GetModel(p => p.Id == id);
            if (role == null)
            {
              return  new RedirectResult("/home/errormsg?msg=未找到该角色");
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
               return new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("未找到该角色"));
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

        [HttpGet]
        public ActionResult RoleMenu(int id)
        {
            if (id == 0)
            {
              return  new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("错误的请求方式！"));
            }
            var roleModel = SystemRoleHelper.GetModel(p => p.Id == id);
            if (roleModel == null)
            {
                new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("不存在的角色！"));
            }
            return View(roleModel);
        }

        [HttpPost]
        public ActionResult Menu(int id)
        {
            if (id == 0)
            {
               return new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("错误的请求方式！"));
            }
            //先查角色
            var roleModel = SystemRoleHelper.GetModel(p => p.Id == id);
            if (roleModel == null)
            {
               return new RedirectResult("/home/errormsg?msg=" + WebUtility.UrlEncode("不存在的角色！"));
            }
            //在查角色对应的菜单
            var menuList = RoleAuthMappingHelper.GetList(p => p.RoleId == id);
            //在查当前用户拥有的菜单
            SystemMenuHelper.Columns.CreatedTime.SetOrderByAsc();
            var allMenu = SystemMenuHelper.GetList(p => p.IsUse == true && CacheUser.AuthMenu.Contains(p.Id), orderBy: SystemMenuHelper.Columns.CreatedTime);
            //递归菜单做成树
            return Json(ResponseHelper.Success(CreateTree(allMenu, menuList, 0)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleMenu([FromForm] AuthSaveMenu model)
        {
            var role = SystemRoleHelper.GetModel(p => p.Id == model.RoleId);
            if (role == null)
            {
                return Json(ResponseHelper.Error("不存在的角色!"));
            }
            var menuList = RoleAuthMappingHelper.GetList(p => p.RoleId == model.RoleId);
            if (menuList.Count > 0)
            {
                var menuStr = string.Join(",", menuList.Select(p => p.MenuId).ToArray());
                if (menuStr.Equals(model.Menus))
                {
                    return Json(ResponseHelper.Success("OK"));
                }
            }
            RoleAuthMappingHelper.Delete(p => p.RoleId == model.RoleId);
            var menuArr = model.Menus.Split(",").ToList();
            List<RoleAuthMapping> insertList = new List<RoleAuthMapping>();
            foreach (var i in menuArr)
            {
                insertList.Add(new RoleAuthMapping
                {
                    CreatedTime = DateTime.Now,
                    MenuId = Convert.ToInt32(i),
                    RoleId = role.Id
                });
            }
            RoleAuthMappingHelper.InsertMany(insertList);
            return Json(ResponseHelper.Success("ok"));
        }

        private List<SystemRoleTreeMenu> CreateTree(List<SystemMenu> allMenu, List<RoleAuthMapping> menuList, int ParentId)
        {
            var TreeModel = new List<SystemRoleTreeMenu>();
            var list = allMenu.Where(p => p.ParentId == ParentId).ToList();
            foreach (var item in list)
            {
                var status = new Dictionary<string, bool>();
                if (item.ActionRoute == "/home/index")
                {
                    status.Clear();
                    status.Add("selected", true);
                    status.Add("disabled", true);
                }
                else if (menuList.Exists(p => p.MenuId == item.Id))
                {
                    status.Add("selected", true);
                }
                var child = CreateTree(allMenu, menuList, item.Id);
                TreeModel.Add(new SystemRoleTreeMenu
                {
                    id = item.Id,
                    children = child,
                    state = status,
                    icon = item.Icon,
                    text = item.Name
                });
            }

            return TreeModel;
        }
    }
}