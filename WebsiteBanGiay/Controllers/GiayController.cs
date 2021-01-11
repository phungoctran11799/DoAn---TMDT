using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class GiayController : Controller
    {
        // GET: Giay
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        public PartialViewResult GiayMoiPartial()
        {
            var lstGiayMoi = db.Giays.Where(n => n.SoLuongTon > 0).Take(3).ToList();
            return PartialView(lstGiayMoi);
        }

        public ViewResult XemChitiet(int _MaGiay = 0)
        {
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == _MaGiay);
            if (giay == null)
            {
                //trả về trang báo lỗi
                Response.StatusCode = 404;
                return null;
            }

            return View(giay);
        }
    }
}