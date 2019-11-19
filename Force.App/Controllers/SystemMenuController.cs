using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.App.Util;
using Force.DataLayer;
using Force.DataLayer.Metadata;
using Force.GenEnum;
using Force.Model;
using Force.Model.ViewModel.Menu;
using Force.Model.ViewModel.SubHeader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class SystemMenuController : BaseController
    {
        public SystemMenuController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }
        // GET: SystemMenu
        public ActionResult Index()
        {
            //查出所有菜单并生成菜单树

            var menu = SystemMenuHelper.GetList();
            var newMenu = CreateMenu(menu, 0);
            var subHeader = new SubHeader { Title = "菜单列表", PageTitle = "菜单列表", SubPageTitle = "所有菜单", LinkText = new Dictionary<string, string>() };
            ViewData["SubHeader"] = subHeader;
            return View(newMenu);
        }

        // GET: SystemMenu/Add
        public ActionResult Add()
        {
            ViewBag.ParentCode = "0";
            if (!string.IsNullOrEmpty(Request.Query["code"]))
            {
                ViewBag.ParentCode = Request.Query["code"].ToString();
            }
            ViewBag.AuthCode = "0";
            return View();
        }

        // POST: SystemMenu/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([FromForm] SaveMenuModel model)
        {
            try
            {
                // TODO: Add insert logic here
                //查询是否存在该菜单
                if (SystemMenuHelper.Exists(p => p.Name == model.ControllName && p.ActionRoute == model.ControllUrl))
                {
                    return Json(ResponseHelper.Error("该菜单已经存在了！"));
                }
                var MenuModel = new SystemMenu
                {
                    ActionRoute = model.ControllUrl.ToLower(),
                    CreatedTime = DateTime.Now,
                    Icon = model.Icon,
                    IsUse = Convert.ToBoolean(model.Status),
                    Name = model.ControllName,
                    ParentId = Convert.ToInt32(model.ParentCode),
                    Remark = model.Remark,
                    Sort = model.Sort,
                    Type = model.ControllType
                };
                SystemMenuHelper.Insert(MenuModel);
                return Json(ResponseHelper.Success("ok"));
            }
            catch
            {
                return Json(ResponseHelper.Error("出现了内部错误！请联系管理员处理！"));
            }
        }

        // GET: SystemMenu/Edit/5
        public ActionResult Edit()
        {
            var useCode = Convert.ToInt32(Request.Query["code"]);
            if (useCode == 0)
            {
                return Redirect("/home/error");
            }
            var data = SystemMenuHelper.GetModel(useCode);
            return View(data);
        }

        // POST: SystemMenu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] SaveMenuModel model)
        {
            try
            {
                // TODO: Add update logic here
                var MenuModel = SystemMenuHelper.GetModel(model.Code);
                //查询是否存在该菜单
                if (SystemMenuHelper.Exists(p => p.Id != model.Code && p.Name == model.ControllName && p.ActionRoute == model.ControllUrl))
                {
                    return Json(new { status = 0, msg = "该菜单已经存在,请修改！" });
                }
                MenuModel.Remark = model.Remark;
                MenuModel.IsUse = Convert.ToBoolean(model.Status);
                MenuModel.Name = model.ControllName;
                MenuModel.Type = model.ControllType;
                MenuModel.ActionRoute = model.ControllUrl;
                MenuModel.Icon = model.Icon;
                MenuModel.Sort = model.Sort;
                MenuModel.ParentId = Convert.ToInt32(model.ParentCode);
                SystemMenuHelper.Update(MenuModel);
                return Json(ResponseHelper.Success("ok"));
            }
            catch
            {
                return Json(ResponseHelper.Error("出现了内部错误！请联系管理员处理！"));
            }
        }
        
        [HttpPost]
        public ActionResult ChangeStatus()
        {
            try {
                var status = Convert.ToInt32(Request.Form["status"]);
                var code = int.Parse(Request.Form["id"]);

                SystemMenuHelper.Update(new SystemMenu { Id = code, IsUse = Convert.ToBoolean(status) }, SystemMenuHelper.Columns.IsUse);
                return new JsonResult(ResponseHelper.Success("ok"));
            } catch
            {
                return new JsonResult(ResponseHelper.Error("出现内部错误请联系管理员解决！"));
            }
            
        }

        // POST: SystemMenu/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static List<MenuViewModel> CreateMenu(List<SystemMenu> AuthList, int ParentId)
        {
            List<MenuViewModel> NewMeun = new List<MenuViewModel>();
            //AuthTypeEnum[] type = new AuthTypeEnum[] { AuthTypeEnum.del, AuthTypeEnum.update };
            var list = AuthList.Where(p => p.ParentId == ParentId).OrderBy(p => p.Sort).ToList();
            if (list.Count < 1)
            {
                return NewMeun;
            }
            foreach (var item in list)
            {
                var type = string.Empty;
                switch (item.Type)
                {
                    case 1:
                        type = "菜单";
                        break;
                    case 2:
                        type = "新增";
                        break;
                    case 3:
                        type = "编辑";
                        break;
                    case 4:
                        type = "删除";
                        break;
                    case 5:
                        type = "查询";
                        break;
                    case 6:
                        type = "页面";
                        break;
                }
                NewMeun.Add(new MenuViewModel()
                {
                    Children = CreateMenu(AuthList, item.Id),
                    MenuCode = item.Id,
                    MenuType = type,
                    Remark = item.Remark,
                    MenuIcon = item.Icon,
                    MenuName = item.Name,
                    MenuUrl = item.ActionRoute,
                    Sort = item.Sort,
                    Status = Convert.ToInt32(item.IsUse)
                });
            }
            return NewMeun;
        }
    }
}