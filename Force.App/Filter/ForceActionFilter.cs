using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Model.ViewModel.Response;
using Microsoft.AspNetCore.Mvc;
using Force.Model.ViewModel.SubHeader;

namespace Force.App.Filter
{
    public class ForceActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ViewResult viewResult)
            {
                if (viewResult.ViewData["SubHeader"] == null)
                {
                    viewResult.ViewData["SubHeader"] = new SubHeader
                    {

                        Title = context.RouteData.Values["controller"].ToString(),
                        LinkText = new Dictionary<string, string>()
                    };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.ModelState.IsValid)
            {
                var result = new MResponse<string>();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        result.Msg += error.ErrorMessage;
                    }
                }

                context.Result = new JsonResult(result);
            }
        }
    }
}
