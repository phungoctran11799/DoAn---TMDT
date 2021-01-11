using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanGiay.Models
{
    public class GioHang
    {
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public int _MaGiay { get; set; }
        public string _TenGiay { get; set; }
        public string _AnhBia { get; set; }
        public double _DonGia { get; set; }
        public int _SoLuong { get; set; }
        public double _ThanhTien
        {
            get { return _SoLuong * _DonGia; }
        }
        //Hàm tạo giỏ hàng
        public GioHang(int __MaGiay)
        {
            _MaGiay = __MaGiay;
            Giay giay = db.Giays.Single(n => n.MaGiay == _MaGiay);//Ở đây chỉ cần dùng single, nếu sai đường dẫn thì bắt ở ctler
            _TenGiay = giay.TenGiay;
            _AnhBia = giay.AnhBia;
            _DonGia = Convert.ToDouble(giay.GiaBan);
            _SoLuong = 1;
        }
    }
}