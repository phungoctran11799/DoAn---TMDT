using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteBanGiay.Controllers
{
    public class QuanLyNhanSuController : Controller
    {
        // GET: QuanLyNhanSu
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 10;
            return View(db.KhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult ChinhSua(int _MaKH)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == _MaKH);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(KhachHang _KhachHang)
        {
            if (!ModelState.IsValid)
            {
                return View(_KhachHang);
            }
            db.Entry(_KhachHang).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HienThi(int _MaKH)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == _MaKH);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpGet]
        public ActionResult Xoa(int _MaKH)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == _MaKH);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateInput(false)]
        public ActionResult XacNhanXoa(int _MaKH)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == _MaKH);
            List<DonHang> lstDonHang = db.DonHangs.Where(n => n.MaKH == _MaKH).ToList();
            if ((khachhang == null) || (lstDonHang.Count > 0) || (_MaKH == 1))
            {
                if (khachhang == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                if ((lstDonHang.Count > 0) || (_MaKH == 1))
                {
                    return View(khachhang);
                }
            }
            db.KhachHangs.Remove(khachhang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}