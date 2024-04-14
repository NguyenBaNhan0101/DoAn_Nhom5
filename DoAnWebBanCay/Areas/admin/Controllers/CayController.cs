using DoAnWebBanCay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;

namespace DoAnWebBanCay.Areas.admin.Controllers
{
    [Authorize(Roles = "admin,Employee")]
    public class CayController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: admin/Cay
        public ActionResult DSCay(int? page, string currentFilter, string searchString)
        {
            var lstCay = new List<Cay>(from tt in data.Cays where tt.SoLuongTon > 0 select tt);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                lstCay = data.Cays.Where(x => x.TenCay.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            else
            {
                lstCay = data.Cays.ToList();
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 6;
            int pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            lstCay = lstCay.OrderByDescending(x => x.MaCay).ToList();
            return View(lstCay.ToPagedList(pageNumber, pageSize));
            //var all_cay = from tt in data.Cays select tt;
            //if (page == null)
            //{
            //    page = 1;
            //}
            //var pageNumber = page ?? 1;
            //var pageSize = 7;
            //ViewBag.PageSize = pageSize;
            //ViewBag.Page = pageNumber;
            //return View(all_cay.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection collection, Cay c)
        {
            var E_tencay = collection["TenCay"];
            var E_hinh = collection["HinhAnh"];
            var E_giaban = Convert.ToDecimal(collection["GiaBan"]);
            var E_soluongton = Convert.ToInt32(collection["SoLuongTon"]);
            if (string.IsNullOrEmpty(E_tencay))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                c.TenCay = E_tencay.ToString();
                c.HinhAnh = E_hinh.ToString();
                c.GiaBan = E_giaban;
                c.SoLuongTon = E_soluongton;
                data.Cays.InsertOnSubmit(c);
                data.SubmitChanges();
                return RedirectToAction("DSCay");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var E_cay = data.Cays.First(m => m.MaCay == id);
            return View(E_cay);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_cay = data.Cays.First(m => m.MaCay == id);
            var E_tencay = collection["TenCay"];
            var E_hinh = collection["HinhAnh"];
            var E_giaban = Convert.ToDecimal(collection["GiaBan"]);
            var E_soluongton = Convert.ToInt32(collection["SoLuongTon"]);
            E_cay.MaCay = id;
            if (string.IsNullOrEmpty(E_tencay))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_cay.TenCay = E_tencay;
                E_cay.HinhAnh = E_hinh;
                E_cay.GiaBan = E_giaban;
                E_cay.SoLuongTon = E_soluongton;
                UpdateModel(E_cay);
                data.SubmitChanges();
                return RedirectToAction("DSCay");
            }
            return this.Edit(id);
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = data.Cays.First(m => m.MaCay == id);
            if (item != null)
            {
                var checkImg = data.Cays.Where(x => x.MaCay == item.MaCay);
                //if (checkImg != null)
                //{
                //    foreach (var img in checkImg)
                //    {
                //        data.Cays.DeleteOnSubmit(img);
                //        data.SubmitChanges();
                //    }
                //}
                data.Cays.DeleteOnSubmit(item);
                data.SubmitChanges();
                return RedirectToAction("DSCay");
                //return Json(new { success = true });
            }
            //return RedirectToAction("DSCay");
            //return Json(new { success = false });
            return View();
        }
    }
}