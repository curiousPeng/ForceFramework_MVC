using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PartialHtml.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}