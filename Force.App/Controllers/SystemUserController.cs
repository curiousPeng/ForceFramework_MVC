using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class SystemUserController : BaseController
    {
        public SystemUserController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        // GET: SystemUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: SystemUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SystemUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemUser/Create
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

        // GET: SystemUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SystemUser/Edit/5
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

        // GET: SystemUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemUser/Delete/5
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