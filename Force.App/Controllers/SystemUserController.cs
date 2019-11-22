using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DataLayer;
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

        //TODO：忙其他事去了。
        //[HttpPost]
        //public ActionResult Index([FromForm])
        //{
        //    int draw = Convert.ToInt32(Request.Form["draw"]);
        //    //分页用的开始ID
        //    int start = Convert.ToInt32(Request.Form["start"]);
        //    //每页数量
        //    int pagesize = Convert.ToInt32(Request.Form["length"]);
        //    var name = string.IsNullOrEmpty(Request.Form["name"]) ? "" : Request.Form["name"].ToString();

        //    var dataList = SystemUserHelper.GetListByPage("Id<>1", " ", pageSize: 10, currentPage: 1);
        //    long total = dataList.TotalPages;
        //    var configList = dataList.Items;

        //    var totalPage = Math.Ceiling(Convert.ToDouble(total / pagesize));
        //    var model = new List<SystemUserViewModel>();
        //    foreach (var item in configList)
        //    {
        //        model.Add(new SystemUserViewModel()
        //        {
        //            UseCode = item.Id,
        //            CreatedTime = item.CreatedTime.ToString("yyyy/MM/dd HH:mm:ss"),
        //            Status = item.Status,
        //            Name = item.Name,
        //            Email = item.Email,
        //            HeadImg = _configuration["ImgURL"] + item.HeadImg,
        //            Phone = item.Phone
        //        });
        //    }
        //    return Json(new { data = model, draw = draw, recordsTotal = total, recordsFiltered = total });
        //}

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