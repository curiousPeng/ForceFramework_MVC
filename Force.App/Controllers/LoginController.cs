using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Common.AES;
using Force.DataLayer;
using Force.Model.ViewModel.Login;
using Force.Model.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Force.App.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult SignIn([FromForm] SignInModel model)
        {
            try
            {
                // TODO: Add login logic here
                var result = new MResponse<string>();
                var password = AESUtil.Md5(model.Password);
                var user = SystemUserHelper.GetModel(p => p.Password == password && p.Email == model.Email);
                if (user == null)
                {
                    result.Msg = "账户或密码错误，请确认后再试！";
                    return Json(result);
                }
                //TODO:暂停在这儿，做其他事情去了！
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
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

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
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