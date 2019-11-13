using Force.DataLayer;
using Force.GenEnum;
using Force.Model;
using Force.Model.ViewModel.Menu;
using Force.Model.ViewModel.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartialHtml.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(GetItemsAsync());
        }
        private List<ViewMenu> GetItemsAsync()
        {
            var UserString = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(UserString))
            {
                return new List<ViewMenu>();
            }
            var SysUser = JsonConvert.DeserializeObject<SessionUser>(UserString);
            var MenuList = new List<SystemMenu>();
            if (SysUser.UId=="1")
            {
                MenuList = SystemMenuHelper.GetList(p => p.Type == SystemMenu_Type_Enum.菜单 && p.IsUse == true);
            }
            else
            {
                MenuList = SystemMenuHelper.GetList(p => SysUser.AuthMenu.Contains(p.Id) && p.Type == SystemMenu_Type_Enum.菜单 && p.IsUse == true);
            }
            //菜单树
            var Menu = GetMenu(MenuList, 0);

            return Menu;
        }
        private List<ViewMenu> GetMenu(List<SystemMenu> MenuList, int ParentCode)
        {
            List<ViewMenu> NewMeun = new List<ViewMenu>();
            var list = MenuList.Where(p => p.ParentId == ParentCode).ToList();
            if (list.Count < 1)
            {
                return NewMeun;
            }
            foreach (var item in list)
            {
                NewMeun.Add(new ViewMenu()
                {
                    Children = GetMenu(MenuList, item.Id),
                    MenuCode = item.Id,
                    MenuIcon = string.IsNullOrEmpty(item.Icon) ? "m-menu__link-bullet--dot" : item.Icon,
                    MenuName = item.Name,
                    Url = string.IsNullOrEmpty(item.ActionRoute) ? "javascript:;" : item.ActionRoute
                });
            }
            return NewMeun;
        }
    }
}