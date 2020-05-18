using Force.Model.ViewModel.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PartialHtml.ViewComponents
{
    public class HeadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var UserString = HttpContext.Session.GetString("UserInfo");
            var SysUser = new SessionUser();
            if (!string.IsNullOrEmpty(UserString))
            {
                SysUser = JsonConvert.DeserializeObject<SessionUser>(UserString);
            }
            return View(SysUser);
        }
    }
}