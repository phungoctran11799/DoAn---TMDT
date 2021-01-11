using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class LoaiController : Controller
    {
        // GET: Loại
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult LoaiPartial()
        {
            return PartialView(db.Loais.Take(3).ToList());
        }
        //Tìm Loại theo Loại
        public ViewResult GiayTheoLoai(int _MaLoai = 0)
        {

            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == _MaLoai); // Loại trả về 1 loại, nếu không có loại đó nó sẽ báo null
            //Kiểm tra loại có tồn tại hay không
            if (loai == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            //Truy xuất các danh Loại theo chủ đề
            List<Giay> lstGiay = db.Giays.Where(n => n.MaLoai == _MaLoai && n.SoLuongTon > 0).OrderBy(n => n.GiaBan).ToList();
            if (lstGiay.Count == 0)
            {
                ViewBag.Giay = "Không có Giày nào thuộc Loại này!";
            }
            return View(lstGiay);
        }
        //Hiển thị các loại khác
        public ViewResult DanhMucLoai()
        {
            return View(db.Loais.ToList());
        }
    }
}