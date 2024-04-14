using DoAnWebBanCay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Controllers
{
    public class CayController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: Cay
        public ActionResult ListCay()
        {
            var all_cay = from tt in data.Cays select tt;
            return View(all_cay);
        }
        public ActionResult Detail(int id)
        {
            var D_cay = data.Cays.Where(m => m.MaCay == id).First();
            return View(D_cay);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Cay c)
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
                return RedirectToAction("ListCay");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var E_cay = data.Cays.First(m => m.MaCay == id);
            return View(E_cay);
        }
        [HttpPost]
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
        //-----------------------------------------
        public ActionResult Delete(int id)
        {
            var D_cay = data.Cays.First(m => m.MaCay == id);
            return View(D_cay);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_cay = data.Cays.Where(m => m.MaCay == id).First();
            data.Cays.DeleteOnSubmit(D_cay);
            data.SubmitChanges();
            return RedirectToAction("ListCay");
        }
    }
}