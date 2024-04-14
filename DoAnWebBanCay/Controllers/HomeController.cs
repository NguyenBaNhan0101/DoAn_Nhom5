using DoAnWebBanCay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index(int? page, string currentFilter , string searchString, string category, string sortBy)
        {
            var lstCay = new List<Cay>(from tt in data.Cays where tt.SoLuongTon > 0 select tt);
            if(searchString != null)
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

            if (!string.IsNullOrEmpty(category))
            {
                lstCay = lstCay.Where(x => x.LoaiCay.TenLoai.ToUpper() == category.ToUpper()).ToList();
            }

            if (sortBy == "price")
            {
                lstCay = lstCay.OrderBy(x => x.GiaBan).ToList();
            }
            else if (sortBy == "price_desc")
            {
                lstCay = lstCay.OrderByDescending(x => x.GiaBan).ToList();
            }

            if (sortBy == "name")
            {
                lstCay = lstCay.OrderBy(x => x.TenCay).ToList();
            }
            else if (sortBy == "name_desc")
            {
                lstCay = lstCay.OrderByDescending(x => x.TenCay).ToList();
            }

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentCategory = category;

            int pageSize = 6;
            int pageNum = page ?? 1;
            lstCay = lstCay.OrderByDescending(x => x.MaCay).ToList();
            return View(lstCay.ToPagedList(pageNum, pageSize));

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    var all_c = data.Cays.Include(l => l.LoaiCay).Where(x => x.TenCay.ToUpper().Contains(searchString.ToUpper()));
            //    return View(all_c.ToList());
            //}
            //if (page == null)
            //    page = 1;
            //    var all_cay = (from s in data.Cays select s).OrderBy(m => m.MaCay);
            //    int pageSize = 6;
            //    int pageNum = page ?? 1;
            //return View(all_cay.ToPagedList(pageNum, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.TT = "Thông tin liên lạc";
            ViewBag.Message = "Địa chỉ";

            return View();
        }
    }
}