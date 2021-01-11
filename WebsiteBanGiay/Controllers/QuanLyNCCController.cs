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
    public class QuanLyNCCController : Controller
    {
        // GET: QuanLyNCC
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 10;
            return View(db.NhaCungCaps.ToList().OrderBy(n => n.MaNCC).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(NhaCungCap _NhaCungCap)
        {
            db.NhaCungCaps.Add(_NhaCungCap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int _MaNCC)
        {
            NhaCungCap NhaCungCap = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == _MaNCC);
            if (NhaCungCap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(NhaCungCap);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(NhaCungCap _NhaCungCap)
        {
            if (!ModelState.IsValid)
            {
                return View(_NhaCungCap);
            }
            db.Entry(_NhaCungCap).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Xoa(int _MaNCC)
        {
            NhaCungCap NhaCungCap = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == _MaNCC);
            if (NhaCungCap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(NhaCungCap);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateInput(false)]
        public ActionResult XacNhanXoa(int _MaNCC)
        {
            NhaCungCap NhaCungCap = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == _MaNCC);
            List<Giay> lstSach = db.Giays.Where(n => n.MaNCC == _MaNCC).ToList();
            if ((NhaCungCap == null) || (lstSach.Count > 0))
            {
                if (NhaCungCap == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                if (lstSach.Count > 0)
                {
                    return View(NhaCungCap);
                }
            }
            db.NhaCungCaps.Remove(NhaCungCap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HienThi(int _MaNCC)
        {
            NhaCungCap NhaCungCap = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == _MaNCC);
            if (NhaCungCap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(NhaCungCap);
        }
    }
}