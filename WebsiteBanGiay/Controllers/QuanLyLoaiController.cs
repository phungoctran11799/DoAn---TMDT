using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;
using PagedList;
using PagedList.Mvc;

namespace WebSiteBanGiay.Controllers
{
    public class QuanLyLoaiController : Controller
    {
        //GET: QuanLyLoai
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 10;
            return View(db.Loais.ToList().OrderBy(n => n.MaLoai).ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Loai _loai)
        {
            db.Loais.Add(_loai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int _MaLoai)
        {
            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == _MaLoai);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Loai _Loai)
        {
            if (!ModelState.IsValid)
            {
                return View(_Loai);
            }
            db.Entry(_Loai).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Xoa(int _MaLoai)
        {
            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == _MaLoai);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateInput(false)]
        public ActionResult XacNhanXoa(int _MaLoai)
        {
            Loai loai = db.Loais.SingleOrDefault(n => n.MaLoai == _MaLoai);
            List<Giay> lstGiay = db.Giays.Where(n => n.MaLoai == _MaLoai).ToList();
            if ((loai == null) || (lstGiay.Count > 0))
            {
                if (loai == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                if (lstGiay.Count > 0)
                {
                    return View(loai);
                }
            }
            db.Loais.Remove(loai);
            db.SaveChanges();
            db.Dispose();
            return RedirectToAction("Index");
        }
    }
}