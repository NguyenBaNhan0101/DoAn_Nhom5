using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DoAnWebBanCay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //MyDataDataContext db = new MyDataDataContext();
        // GET: admin/Role
        public ActionResult DSRole()
        {
            var items = db.Roles.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Create(model);
                return RedirectToAction("DSRole");
            }
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            var item = db.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Update(model);
                return RedirectToAction("DSRole");
            }
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            var item = db.Roles.First(m => m.Id == id);
            if (item != null)
            {
                var checkImg = db.Roles.Where(x => x.Id == item.Id);
                //if (checkImg != null)
                //{
                //    foreach (var img in checkImg)
                //    {
                //        data.Cays.DeleteOnSubmit(img);
                //        data.SubmitChanges();
                //    }
                //}
                db.Roles.Remove(item);
                db.SaveChanges();
                return RedirectToAction("DSRole");
                //return Json(new { success = true });
            }
            return RedirectToAction("DSRole");
            //return Json(new { success = false });
            //return View();
        }
    }
}