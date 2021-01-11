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
    public class HomeController : Controller
    {
        QuanLyBanGiayModel db = new QuanLyBanGiayModel(); // Khởi tạo database

        public ActionResult Index(int? _Page, string err)
        {
            if (err != null)
            {
                ViewBag.Thongbao = err;
            }
            else
            {
                ViewBag.Thongbao = null;
            }

            int PageSize = 12;
            int PageNumber = (_Page ?? 1);
            return View(db.Giays.Where(n => n.SoLuongTon > 0).OrderBy(n => n.MaGiay).ToPagedList(PageNumber, PageSize));
        }

        public PartialViewResult GiayMoiPartial()
        {
            var lstGiayMoi = db.Giays.Take(15).ToList(); //Câu truy vấn để lấy số lượng giày mới
            return PartialView(lstGiayMoi);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}