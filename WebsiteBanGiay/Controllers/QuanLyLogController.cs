using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class QuanLyLogController : Controller
    {
        // GET: QuanLyLog
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 20;
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null) return RedirectToAction("Index", "Home");
            using (var db = new QuanLyBanGiayModel())
            {
                db.Logs.Add(new Log
                {
                    Email = kh.Email,
                    Time = DateTime.Now,
                    Message = $"Quản Trị Viên {kh.HoTen} đã vừa đăng nhập vào lúc {DateTime.Now}"
                });
                ViewBag.Logs = db.Logs.OrderByDescending(log => log.Time).ToList();
                db.SaveChanges();
            }
            return View(db.Logs.ToList().OrderByDescending(n => n.ID).ToPagedList(PageNumber, PageSize));
        }
        public ActionResult Details(int id)
        {
            Log log = db.Logs.SingleOrDefault(n => n.ID == id);
            return View(log);
        }
    }
}