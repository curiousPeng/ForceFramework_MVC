 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DataLayer;
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
        public ActionResult Add([FromBody] SaveMenuModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
        public ActionResult Edit([FromBody] SaveMenuModel model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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