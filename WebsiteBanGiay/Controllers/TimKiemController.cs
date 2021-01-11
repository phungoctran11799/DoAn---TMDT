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
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection fc, int? _Page)
        {
            string TuKhoa = fc["txtTimKiem"].ToString().Trim();
            ViewBag.TuKhoa = TuKhoa;
            List<Giay   > lstGiay = db.Giays.Where(n => n.TenGiay.Contains(TuKhoa) && n.SoLuongTon > 0).ToList();
            int pageNumber = (_Page ?? 1);
            int pageSize = 9;
            if (lstGiay.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy Giày bạn yêu cầu !";
                return View(db.Giays.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstGiay.Count.ToString() + " giày :";
            return View(lstGiay.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(string _TuKhoa, int? _Page)
        {
            ViewBag.TuKhoa = _TuKhoa;
            List<Giay> lstGiay = db.Giays.Where(n => n.TenGiay.Contains(_TuKhoa) && n.SoLuongTon > 0).ToList();
            int pageNumber = (_Page ?? 1);
            int pageSize = 9;
            if (lstGiay.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy giày bạn yêu cầu !";
                return View(db.Giays.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstGiay.Count.ToString() + " giày :";
            return View(lstGiay.OrderBy(n => n.TenGiay).ToPagedList(pageNumber, pageSize));
        }
    }
}