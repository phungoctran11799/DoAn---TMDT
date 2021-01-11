using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: NhaCungCap
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public PartialViewResult NhaCungCapPartial()
        {
            return PartialView(db.NhaCungCaps.Take(6).OrderBy(ncc => ncc.MaNCC).ToList());
        }
        //Hiển thị sách theo nhà cung cấp
        public ViewResult GiayTheoNCC(int _MaNCC)
        {
            //Kiểm tra NCC có tồn tại không
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == _MaNCC);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            //Truy  Xuất danh sách các sách NCC
            List<Giay> lstGiay = db.Giays.Where(n => n.MaNCC == _MaNCC && n.SoLuongTon > 0).OrderBy(n => n.GiaBan).ToList();
            //Xem thử có NCC nào đó không
            if (lstGiay.Count == 0)
            {
                ViewBag.Giay = "Không có Giày của Nhà cung cấp này!";
            }
            return View(lstGiay);
        }
        //Hiển thị các NCC
        public ViewResult DanhMucNCC()
        {
            return View(db.NhaCungCaps.ToList());
        }
    }
}