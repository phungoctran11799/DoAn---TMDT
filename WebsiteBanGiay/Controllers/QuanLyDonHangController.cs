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
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 10;
            return View(db.DonHangs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult ChinhSua(int _MaDonHang)
        {
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == _MaDonHang);
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donhang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(DonHang _DonHang)
        {
            if (!ModelState.IsValid)
            {
                return View(_DonHang);
            }
            db.Entry(_DonHang).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}