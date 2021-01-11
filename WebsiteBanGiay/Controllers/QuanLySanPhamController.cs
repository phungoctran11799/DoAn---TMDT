using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanGiay.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanGiayModel db = new QuanLyBanGiayModel();
        //Trang hiển thi
        public ActionResult Index(int? _Page)
        {
            int PageNumber = (_Page ?? 1);
            int PageSize = 5;
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null) return RedirectToAction("Index", "Home");
            ViewBag.Logs = db.Logs.OrderByDescending(log => log.Time).ToList();
            return View(db.Giays.ToList().OrderByDescending(n => n.MaGiay).ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            //Đưa chủ đề vào dropdownList
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        //Thêm mới
        public ActionResult ThemMoi(Giay _Giay, HttpPostedFileBase FileUpload)
        {

            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            if (FileUpload == null)
            {
                ViewBag.ThongBao = "Chưa chọn Ảnh bìa";
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(_Giay);
            }
            //Lưu tên và đường dẫn của file
            var FileName = Path.GetFileName(FileUpload.FileName);
            var DuongDan = Path.Combine(Server.MapPath("~/HinhAnhSP"), FileName);
            //Kiểm tra hình ảnh đã tồn tại chưa           
            if (!System.IO.File.Exists(DuongDan))
            {
                FileUpload.SaveAs(DuongDan);
            }
            _Giay.AnhBia = FileUpload.FileName;
            _Giay.NgayCapNhat = DateTime.Now;
            db.Giays.Add(_Giay);
            db.SaveChanges();

            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null) return RedirectToAction("Index", "Home");
            using (var db = new QuanLyBanGiayModel())
            {
                db.Logs.Add(new Log
                {
                    Email = kh.Email,
                    Time = DateTime.Now,
                    Message = $"Quản Trị Viên {kh.HoTen} đã vừa THÊM giày: {_Giay.TenGiay} vào lúc {DateTime.Now}"
                });
                ViewBag.Logs = db.Logs.OrderByDescending(log => log.Time).ToList();
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //Chỉnh sửa
        [HttpGet]
        public ActionResult ChinhSua(int _MaGiay)
        {
            // lấy ra đối tượng giày theo mã
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == _MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", giay.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "NhaCC", "TenNCC", giay.MaNCC);
            return View(giay);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Giay _Giay, HttpPostedFileBase FileUpload)
        {
            //đưa dữ liệu vào dropdownlist
            ViewBag.MaLoai = new SelectList(db.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", _Giay.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "NhaCC", "TenNCC", _Giay.MaNCC);
            //kiểm tra ảnh bìa
            if (FileUpload == null)
            {
                ViewBag.ThongBao = "Chưa chọn Ảnh bìa";
                return View(_Giay);
            }
            //đưa vào csdl
            if (!ModelState.IsValid)
            {
                return View(_Giay);
            }
            var FileName = Path.GetFileName(FileUpload.FileName);
            var DuongDan = Path.Combine(Server.MapPath("~/HinhAnhSP"), FileName);
            if (!System.IO.File.Exists(DuongDan))
            {
                FileUpload.SaveAs(DuongDan);
            }
            _Giay.AnhBia = FileUpload.FileName;
            // thực hiện cập nhật model 
            db.Entry(_Giay).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null) return RedirectToAction("Index", "Home");
            using (var db = new QuanLyBanGiayModel())
            {
                db.Logs.Add(new Log
                {
                    Email = kh.Email,
                    Time = DateTime.Now,
                    Message = $"Quản Trị Viên {kh.HoTen} đã vừa CHỈNH SỬA giày {_Giay.TenGiay} vào lúc {DateTime.Now}"
                });
                ViewBag.Logs = db.Logs.OrderByDescending(log => log.Time).ToList();
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult HienThi(int _MaGiay)
        {
            //Lấy danh giày theo mã
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == _MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        [HttpGet]
        //Xóa
        public ActionResult Xoa(int _MaGiay)
        {
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == _MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateInput(false)]
        public ActionResult XacNhanXoa(int _MaGiay)
        {
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == _MaGiay);
            db.Giays.Remove(giay);
            db.SaveChanges();

            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh == null) return RedirectToAction("Index", "Home");
            using (var db = new QuanLyBanGiayModel())
            {
                db.Logs.Add(new Log
                {
                    Email = kh.Email,
                    Time = DateTime.Now,
                    Message = $"Quản Trị Viên {kh.HoTen} đã vừa XÓA giày  {giay.TenGiay } vào lúc {DateTime.Now}"
                });

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
