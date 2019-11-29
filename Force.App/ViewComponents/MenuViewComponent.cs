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
            MenuList = SystemMenuHelper.GetList(p => SysUser.AuthMenu.Contains(p.Id) && p.Type == SystemMenu_Type_Enum.菜单 && p.IsUse == true);
            //菜单树
            var NowAction = HttpContext.Request.Path.ToString().ToLower();
            var Action = MenuList.Where(p => p.ActionRoute == NowAction).Last();
            var Menu = GetMenu(MenuList, 0, NowAction);
            UpdateParentMenuChecked(Menu, Action.ParentId);
            ViewBag.NowAction = NowAction;
            return Menu;
        }
        private List<ViewMenu> GetMenu(List<SystemMenu> MenuList, int ParentCode, string NowAction)
        {
            List<ViewMenu> NewMeun = new List<ViewMenu>();
            var list = MenuList.Where(p => p.ParentId == ParentCode && p.ActionRoute != "/home/index").ToList();
            if (list.Count < 1)
            {
                return NewMeun;
            }
            foreach (var item in list)
            {
                bool IsChecked = false;
                if (NowAction == item.ActionRoute)
                {
                    IsChecked = true;
                }
                NewMeun.Add(new ViewMenu()
                {
                    Children = GetMenu(MenuList, item.Id, NowAction),
                    MenuCode = item.Id,
                    ParentCode = ParentCode,
                    MenuIcon = string.IsNullOrEmpty(item.Icon) ? "glyphicon glyphicon-asterisk" : item.Icon,
                    MenuName = item.Name,
                    IsChecked = IsChecked,
                    Url = string.IsNullOrEmpty(item.ActionRoute) ? "javascript:;" : item.ActionRoute
                });
            }
            return NewMeun;
        }

        private void UpdateParentMenuChecked(List<ViewMenu> MenuList,int ParentId)
        {
           foreach(var item in MenuList)
            {
               
                if (item.MenuCode == ParentId)
                {
                    item.IsChecked = true;
                    if (item.ParentCode != 0)
                    {
                        UpdateParentMenuChecked(MenuList, item.ParentCode);
                    }
                    return;
                }
                else
                {
                    if (item.Children.Count > 0)
                    {
                        UpdateParentMenuChecked(item.Children, ParentId);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}