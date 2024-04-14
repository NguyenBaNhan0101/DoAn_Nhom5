using DoAnWebBanCay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWebBanCay.Areas.admin.Controllers
{
    [Authorize(Roles = "admin,Employee")]
    public class OrderController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: admin/Order
        public ActionResult DSDonHang(int? page, string currentFilter, string searchString)
        {
            var lstDonHang = new List<DonHang>(from tt in db.DonHangs select tt);
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
                lstDonHang = db.DonHangs.Where(x => x.UserName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            else
            {
                lstDonHang = db.DonHangs.ToList();
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 6;
            int pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            lstDonHang = lstDonHang.OrderByDescending(x => x.MaDonHang).ToList();
            return View(lstDonHang.ToPagedList(pageNumber, pageSize));
            //var all_dh = from tt in db.DonHangs select tt;
            //if(page == null)
            //{
            //    page = 1;
            //}
            //var pageNumber = page ?? 1;
            //var pageSize = 7;
            //ViewBag.PageSize = pageSize;
            //ViewBag.Page = pageNumber;
            //return View(all_dh.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult View(int id)
        {
            var item = db.DonHangs.Where(m => m.MaDonHang == id).First();
            return View(item);
        }
        public ActionResult Partial_SanPham(int id)
        {
            var items = db.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
            return PartialView(items);
        }
        //[HttpPost]
        //public ActionResult ConfirmOrder(int id)
        //{
        //    try
        //    {
        //        // Lấy thông tin đơn hàng từ CSDL dựa trên orderId
        //        var order = db.DonHangs.FirstOrDefault(o => o.MaDonHang == id);

        //        if (order == null)
        //        {
        //            // Không tìm thấy đơn hàng, trả về thông báo lỗi
        //            TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
        //            return RedirectToAction("Index");
        //        }

        //        // Thực hiện các thao tác cần thiết để xác nhận đơn hàng, ví dụ:
        //        order.TrangThai = "Đã xác nhận";
        //        db.SubmitChanges();

        //        TempData["SuccessMessage"] = "Đơn hàng đã được xác nhận thành công.";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xảy ra lỗi trong quá trình xử lý, trả về thông báo lỗi
        //        TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại sau.";
        //        return RedirectToAction("Index");
        //    }
        //}
        [HttpPost]
        public ActionResult ConfirmOrder(int orderId)
        {
            var order = db.DonHangs.FirstOrDefault(o => o.MaDonHang == orderId);
            if (order != null)
            {
                order.TrangThai = "Đã xác nhận";
                db.SubmitChanges();
                return RedirectToAction("DSDonHang");
            }
            return View();
        }
    }
}