using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DoAnWebBanCay.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int MaCay { get; set; }
        [Display(Name = "Tên cây")]
        public string TenCay { get; set; }
        [Display(Name = "Hình Ảnh ")]
        public string HinhAnh { get; set; }
        [Display(Name = "Giá bán ")]
        public double giaban { get; set; }
        [Display(Name = "Số lượng ")]
        public int soluong { get; set; }
        [Display(Name = "Thành Tiền")]
        public double ThanhTien { get { return soluong * giaban; } }
        public GioHang(int id)
        {
            MaCay = id;
            Cay cay = data.Cays.Single(n => n.MaCay == MaCay);
            TenCay = cay.TenCay;
            HinhAnh = cay.HinhAnh;
            giaban = double.Parse(cay.GiaBan.ToString());
            soluong = 1;
        }
    }
}