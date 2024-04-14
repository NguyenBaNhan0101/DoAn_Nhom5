using DoAnWebBanCay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: admin/Category
        public ActionResult DSDanhMuc(int? page, string currentFilter, string searchString)
        {
            var lstCay = new List<LoaiCay>(from tt in data.LoaiCays select tt);
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
                lstCay = data.LoaiCays.Where(x => x.TenLoai.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            else
            {
                lstCay = data.LoaiCays.ToList();
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 6;
            int pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            lstCay = lstCay.OrderByDescending(x => x.MaLoai).ToList();
            return View(lstCay.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection collection, LoaiCay l)
        {
            var E_tenloai = collection["TenLoai"];
            if (string.IsNullOrEmpty(E_tenloai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                l.TenLoai = E_tenloai.ToString();
                data.LoaiCays.InsertOnSubmit(l);
                data.SubmitChanges();
                return RedirectToAction("DSDanhMuc");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var E_loai = data.LoaiCays.First(m => m.MaLoai == id);
            return View(E_loai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_loai = data.LoaiCays.First(m => m.MaLoai == id);
            var E_tenloai = collection["TenLoai"];
            E_loai.MaLoai = id;
            if (string.IsNullOrEmpty(E_tenloai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_loai.TenLoai = E_tenloai;
                UpdateModel(E_loai);
                data.SubmitChanges();
                return RedirectToAction("DSDanhMuc");
            }
            return this.Edit(id);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = data.LoaiCays.First(m => m.MaLoai == id);
            if (item != null)
            {
                var checkImg = data.LoaiCays.Where(x => x.MaLoai == item.MaLoai);
                //if (checkImg != null)
                //{
                //    foreach (var img in checkImg)
                //    {
                //        data.Cays.DeleteOnSubmit(img);
                //        data.SubmitChanges();
                //    }
                //}
                data.LoaiCays.DeleteOnSubmit(item);
                data.SubmitChanges();
                return RedirectToAction("DSDanhMuc");
                //return Json(new { success = true });
            }
            //return RedirectToAction("DSCay");
            //return Json(new { success = false });
            return View();
        }
    }
}