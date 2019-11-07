using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Model.ViewModel.SubHeader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor){}

        // GET: Home
        public ActionResult Index()
        {
            var subHeader = new SubHeader { Title = "主页", LinkText = new Dictionary<string, string> { { "主页", "/home/index" } } };
            ViewData["SubHeader"] = subHeader;
            return View();
        }

        public ActionResult Error(int ErrorCode)
        {
            switch (ErrorCode)
            {
                case StatusCodes.Status404NotFound:
                    return View("Error404");
                case StatusCodes.Status401Unauthorized:
                case StatusCodes.Status402PaymentRequired:
                case StatusCodes.Status403Forbidden:
                    return View("Error401");
                default:
                   return View("Error");
            }
        }
        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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
    }
}