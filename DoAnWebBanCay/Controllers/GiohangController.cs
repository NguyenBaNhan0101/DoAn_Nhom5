using DoAnWebBanCay.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace DoAnWebBanCay.Controllers
{
    [Authorize()]
    public class GiohangController : Controller
    {
        // GET: Giohang
        MyDataDataContext data = new MyDataDataContext();

        //Lay thong tin gio hang
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Them vao gio hang
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.Find(n => n.MaCay == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                TempData["Message"] = "Sản phẩm đã được thêm vào giỏ hàng.";
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong  trong gio hang
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.soluong);
            }
            return tsl;
        }
        //Tong so luong loai sp trong gio hang
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Count;
            }
            return tsl;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.ThanhTien);
            }
            return tt;
        }
        //Gio hang
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            foreach (var item in lstGioHang)
            {
                var cay = data.Cays.SingleOrDefault(x => x.MaCay == item.MaCay);
                ViewBag.SoLuongTon = cay.SoLuongTon;
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsp = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsp = TongSoLuongSanPham();
            return PartialView();
        }
        //Xoa gio hang
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.MaCay == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.MaCay == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        //Cap nhat gio hang
        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.MaCay == id);
            if (sanpham != null)
            {
                sanpham.soluong = int.Parse(collection["txtSolg"].ToString());
                TempData["Message"] = "Số lượng sản phẩm đã được cập nhật.";
            }
            return RedirectToAction("GioHang");
        }
        //Xoa tat ca gio hang
        public ActionResult XoaAllGioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        //Dat hang
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsp = TongSoLuongSanPham();

            if (lstGioHang == null || lstGioHang.Count == 0)
            {
                TempData["Warning"] = "Giỏ hàng trống! Hãy thêm sản phẩm trước khi đặt hàng";
                return RedirectToAction("GioHang", "GioHang");
            }

            foreach (var item in lstGioHang)
            {
                var cay = data.Cays.SingleOrDefault(s => s.MaCay == item.MaCay);
                if (cay != null)
                {
                    cay.SoLuongTon -= item.soluong;
                }
            }
            data.SubmitChanges();

            //List<GioHang> gh = Laygiohang();
            DonHang dh = new DonHang();
            var db = from tt in data.AspNetUsers where tt.Id.Trim() == User.Identity.GetUserId() select tt;

            dh.IdUser = User.Identity.GetUserId();
            dh.UserName = User.Identity.Name;
            dh.Email = db.First().Email;
            dh.Number = db.First().PhoneNumber;
            dh.SoLuong = ViewBag.Tongsoluong;
            dh.TongThanh = (decimal)ViewBag.TongTien;
            dh.NgayDat = DateTime.Now;
            dh.TrangThai = "Chờ xác nhận";
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();

            foreach (var item in lstGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaCay = item.MaCay;
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.TenCay = item.TenCay;
                ctdh.Soluong = item.soluong;
                ctdh.GiaTien = (decimal)item.giaban;
                ctdh.TongTien = (decimal)item.ThanhTien;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            //List<GioHang> lstGioHang = Laygiohang();
            //foreach (var item in lstGioHang)
            //{
            //    var cay = data.Cays.SingleOrDefault(s => s.MaCay == item.MaCay);
            //    if (cay != null)
            //    {
            //        cay.SoLuongTon -= item.soluong;
            //    }
            //}
            //data.SubmitChanges();

            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}