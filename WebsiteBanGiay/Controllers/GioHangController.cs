using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebsiteBanGiay.Common;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class GioHangController : Controller
    {

        QuanLyBanGiayModel db = new QuanLyBanGiayModel();

        #region Gio hang
        //Lấy giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //Nếu giỏi hàng chưa tồn tại thì tiến hành khởi tạo list giỏ hàng (sessionGioHang) (session dùng để lưu đến khi tắt trang web)
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng
        public ActionResult ThemGioHang(int __MaGiay, string strURL)
        {
            // kiểm tra mã giày có hay không
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == __MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm này tồn tại trong session[giohang] chưa
            GioHang _GioHang = lstGioHang.Find(n => n._MaGiay == __MaGiay);
            if (_GioHang == null)
            {
                _GioHang = new GioHang(__MaGiay);
                //Thêm sản phẩm vào list
                lstGioHang.Add(_GioHang);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (_GioHang._SoLuong <giay.SoLuongTon)
                {
                    _GioHang._SoLuong++;

                }
                else
                {

                    string err = "SẢN PHẨM BẠN VỪA CHỌN ĐÃ HẾT HÀNG :(";
                    return RedirectToAction("Index", "Home", new { @err = err });
                }


                return RedirectToAction("Index", "Home");
            }
        }
        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(int __MaGiay, FormCollection fc)
        {
            //Kiểm tra mã sp
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == __MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<GioHang> lstGioHang = LayGioHang();// lấy giỏ hàng ra từ session
            //Kiểm tra sản phẩm có tồn tại trong session[GioHang]
            GioHang giohang = lstGioHang.SingleOrDefault(n => n._MaGiay == __MaGiay);
            //Nếu mà tồn tại, sửa số lượng
            if (giohang != null)
            {
                int SoLuong = Convert.ToInt32(fc["txtSoLuong"].ToString());
                if (SoLuong > 0)
                {
                    if (SoLuong >= giay.SoLuongTon)
                    {
                        giohang._SoLuong = Convert.ToInt32(giay.SoLuongTon);
                    }
                    else
                    {
                        giohang._SoLuong = SoLuong;
                    }
                }
                else
                {
                    lstGioHang.RemoveAll(n => n._MaGiay == __MaGiay);
                }
                if (lstGioHang.Count == 0)
                {
                    Session["GioHang"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("GioHang");
                }
            }
            return RedirectToAction("GioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int __MaGiay)
        {
            // kiểm tra mã sp
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == __MaGiay);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng ra từ session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong session[GioHang]
            GioHang giohang = lstGioHang.SingleOrDefault(n => n._MaGiay == __MaGiay);
            //NẾu tồn tại thì cho xóa
            if (giohang != null)
            {
                lstGioHang.RemoveAll(n => n._MaGiay == __MaGiay);
            }
            //Nếu giỏ hàng rỗng trả về home
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        //Tính tổng số lượng và tổng tiền
        private int TongSoLuong()
        {
            int _TongsoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                _TongsoLuong = lstGioHang.Sum(n => n._SoLuong);
            }
            return _TongsoLuong;
        }

        private double TongTien()
        {
            double _TongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                _TongTien = lstGioHang.Sum(n => n._ThanhTien);
            }
            return _TongTien;
        }
        // tạo partial giỏ hàng
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public ActionResult GioHangTongTien()
        {
            if (TongTien() <= 0)
            {
                return PartialView();
            }
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng view cho người dùng chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        #endregion

        #region Dat hang
        //Xây dựng chức năng đặt hàng
        [HttpPost]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            //thêm đơn hàng
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            dh.MaKH = kh.MaKH;
            dh.DaThanhToan = Convert.ToInt32(TongTien());
            dh.TinhTrangGiaoHang = 0;
            dh.NgayDat = DateTime.Now;
            dh.NgayGiao = DateTime.Now;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng 
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaGiay = item._MaGiay;
                ctdh.SoLuong = item._SoLuong;
                ctdh.DonGia = item._DonGia.ToString();


                Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == item._MaGiay);
                giay.SoLuongTon -= item._SoLuong;
                db.ChiTietDonHangs.Add(ctdh);
                db.SaveChanges();
                Session["GioHang"] = null;
            }

            //momopay
            string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
            string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
            string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
            string notifyurl = ConfigurationManager.AppSettings["notifyurl"].ToString();

            string amount = gh.Sum(n => n._ThanhTien).ToString();
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
            //mômopay

        }

        public ActionResult HoaDon()
        {
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.KhachHang = (KhachHang)Session["TaiKhoan"];
            return View(lstGioHang);
        }
        public ActionResult ThongBao()
        {
            return View();
        }
        public ActionResult ThanhToan()
        {
            List<GioHang> gioHang = Session["Cart"] as List<GioHang>;
            string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
            string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
            string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
            string notifyurl = ConfigurationManager.AppSettings["notifyurl"].ToString();

            string amount = gioHang.Sum(n => n._ThanhTien).ToString();
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //before sign HMAC SHA256 signature abc
            string rawHash = "partnerCode" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo" +
                orderInfo + "&returnUrl" +
                returnUrl + "&notifyUrl" +
                notifyurl + "&extraData" +
                extraData;
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode" , partnerCode },
                { "accessKey" , accessKey },
                { "requestId" , requestId },
                { "amount" , amount },
                { "orderId" , orderid },
                { "orderInfo" , partnerCode },
                { "returnUrl" , partnerCode },
                { "notifyUrl" , notifyurl },
                { "requestType" , "captureMoMoWallet" },
                { "signature" , signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            string serctkey = ConfigurationManager.AppSettings["serctkey"].ToString();
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(param, serctkey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = " Thông tin Request không hợp lệ";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại";
            }
            else
            {
                ViewBag.message = "Thanh toán thành công";
                Session["Cart"] = new List<GioHang>();
            }
            return View();
        }
        public JsonResult NotifyUrl()
        {
            string param = ""; // Request.Form.ToString().Substring(0, Request.Form.ToString().IndexOf("signature") -1);
            param = "partner_Code=" + Request["partner_code"] +
                "&access_key=" + Request["access_key"] +
                "&amount=" + Request["access_key"] +
                "&order_id=" + Request["order_id"] +
                "&order_info=" + Request["order_info"] +
                "&order_type=" + Request["order_type"] +
                "&transaction_id=" + Request["transaction_id"] +
                "&message=" + Request["message"] +
                "&response_time=" + Request["response_time"] +
                "&status_code=" + Request["status_code"];
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectkey);
            //Không được phép cập nhật trạng thái đơn khi trạng đơn trong databasse khác trạng đang chờ thanh toán
            //Trạng thái đơn kích nút thanh toán - Đang chờ thanh toán(-1)
            //Trạng thái giao dịch thành công(1)
            //Trạng thái giao dịch thất bại(0)
            if (signature != Request["signature"].ToString())
            {
                //Kiểm tra đơn hàng của các bạn trong database có khác trạng thái đang chờ thanh toán hay không 
                //Nếu mà bạn đã cập nhật trạng thái đơn hàng về (1) hoặc 0 rồi thì không cần phải cập nhật nữa
                //Nếu trạng thái đơn hàng của các bạn đang là chờ thanh toán thì các ạn cập nhật trạng thái = 0 là thất bại ở đây

                //Khi nào thì mới cập nhật trạng thái đơn hàng?
            }
            string status_code = Request["status_code"].ToString();
            if ((status_code != "0"))
            {
                //Thất bại - Cập nhật trạng thái đơn hàng
            }
            else
            {
                //Thành công - Cập nhật trạng thái đơn hàng
            }
            return Json("", JsonRequestBehavior.AllowGet);




        }

        #endregion
    }
}