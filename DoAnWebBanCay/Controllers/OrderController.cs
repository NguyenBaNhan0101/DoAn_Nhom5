using DoAnWebBanCay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Controllers
{
    public class OrderController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: DonHang
        public ActionResult DSDH()
        {
            var all_dh = from tt in db.DonHangs where tt.UserName == User.Identity.Name select tt;
            return View(all_dh);
        }
        public ActionResult View(int id)
        {
            var item = db.DonHangs.Where(m => m.MaDonHang == id).First();
            return View(item);
        }
        public ActionResult Partial_SP(int id)
        {
            var items = db.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
            return PartialView(items);
        }
    }
}