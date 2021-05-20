using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;

namespace WebsiteBanGiaySneaker.Controllers
{
    public class GioHangController : Controller
    {

        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();

        #region Giỏ hàng
        //Lấy giỏi hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            //nếu giỏi hàng chưa tồn tại
            if (listGioHang == null)
            {
                //khỏi tạo giỏ hàng
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        //Thêm giỏi hàng
        public ActionResult ThemGioHang(int masp, string URL)
        {
            //lấy mã màu mã  size từ dropdownlist
            int mau = int.Parse(Request.Form["MauSac"]);
            int sizes = int.Parse(Request.Form["Size"]);
            //kiểm tra sự tồn tại của giày
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //kiểm tra tồn  tại màu sắc và size của đôi giày
            CHITIETSP giay1 = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == masp && n.MaMau == mau && n.MaSize == sizes);
            if (giay1 == null)
            {
                TempData["kthanhcong"] = "Sản phẩm hiện hết hàng. Bạn thử lựa chọn màu sắc hoặc size khác!";
                return RedirectToAction("XemChiTiet", "SanPham", new { @masp = giay.MaSP, @mansx = giay.MaNSX });
            }
            //Lấy ra session giỏ hàng
            List<GioHang> listGioHang = LayGioHang();
            //Kiểm tra sản phẩm đã tồn tại trong giỏ hàng chưa?
            GioHang sanpham = listGioHang.Find(n => n.masp == masp && n.mamau == mau && n.masize == sizes);
            var slgiay = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == masp && n.MaMau == mau && n.MaSize == sizes).SoLuong;
            TempData["TongSoLuong"] = TongSoLuong();
            if (TongSoLuongSP(masp) >= slgiay && sanpham != null)
            {
                TempData["loisl"] = "Sản phẩm hiện không đủ số lượng yêu cầu. Vui lòng chọn ít hơn!";
                ModelState.AddModelError("loisl", " ");
                return RedirectToAction("XemChiTiet", "SanPham", new { @masp = giay.MaSP, @mansx = giay.MaNSX });
            }
            else if (TongSoLuongSP(masp) >= 10 && sanpham != null)
            {
                TempData["loisl"] = "Không được mua lớn hơn 10 sản phẩm, vui lòng liên hệ chủ Shop để được ưu đãi!";
                ModelState.AddModelError("loisl", " ");
                return RedirectToAction("XemChiTiet", "SanPham", new { @masp = giay.MaSP, @mansx = giay.MaNSX });
            }
            else if (sanpham == null)
            {
                sanpham = new GioHang(masp, mau, sizes);
                //Add sản phẩm mới mua vào list
                listGioHang.Add(sanpham);
                TempData["thanhcong"] = "Thêm vào giỏ hàng thành công";
                return RedirectToAction("XemChiTiet", "SanPham", new { @masp = giay.MaSP, @mansx = giay.MaNSX });
            }
            else
            {
                sanpham.soluong++;
                TempData["thanhcong"] = "Thêm vào giỏ hàng thành công";
                return RedirectToAction("XemChiTiet", "SanPham", new { @masp = giay.MaSP, @mansx = giay.MaNSX });
            }
        }

        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(int masp, int mamau, int masize, FormCollection f)
        {
            int kiemtra = Int32.Parse(Request.Form["txtSoLuong"]);
            var slgiay = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == masp && n.MaMau == mamau && n.MaSize == masize).SoLuong;
            if (kiemtra > slgiay)
            {
                TempData["loisl"] = "Sản phẩm hiện kông đủ số lượng yêu cầu. Vui lòng chọn ít hơn!";
                ModelState.AddModelError("loisl", " ");
                return RedirectToAction("GioHang", "GioHang");
            }
            else if (kiemtra > 10)
            {
                TempData["loisl"] = "Không thể mua số lượng lớn hơn 10 sản phẩm. Vui lòng liên hệ chủ Shop để được hỗ trợ!";
                ModelState.AddModelError("loisl", " ");
                return RedirectToAction("GioHang", "GioHang");
            }
            else if (kiemtra < 1)
            {
                TempData["loisl"] = "Không thể mua số lượng nhỏ hơn 1 sản phẩm. Vui lòng chọn lại!";
                ModelState.AddModelError("loisl", " ");
                return RedirectToAction("GioHang", "GioHang");
            }
            else
            {
                SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
                if (giay == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                List<GioHang> listGioHang = LayGioHang();
                GioHang sanpham = listGioHang.SingleOrDefault(n => n.masp == masp && n.mamau == mamau && n.masize == masize);
                if (sanpham != null)
                {
                    sanpham.soluong = int.Parse(f["txtSoLuong"].ToString());
                }
                return RedirectToAction("GioHang", "GioHang");
            }
        }
        //Ktra sản phẩm



        //Xóa Giỏ hàng
        public ActionResult XoaGioHang(int masp, int mamau, int masize)
        {
            //Ktra sản phẩm
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listGioHang = LayGioHang();
            GioHang sanpham = listGioHang.SingleOrDefault(n => n.masp == masp && n.mamau == mamau && n.masize == masize);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.masp == masp && n.mamau == mamau && n.masize == masize);

            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "CuaHang");
            }
            return RedirectToAction("GioHang");
        }

        //Trang giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "CuaHang");
            }
            List<GioHang> listGioHang = LayGioHang();
            TempData["TongTien"] = TongTien();
            return View(listGioHang);
        }

        //Tính tổng số lượng
        private int TongSoLuong()
        {
            int tongsoluong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tongsoluong = listGioHang.Sum(n => n.soluong);
            }
            return tongsoluong;
        }

        // //Tính tổng số lượng theo sp
        private int TongSoLuongSP(int masp)
        {
            int tongsoluong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tongsoluong = listGioHang.Where(n => n.masp == masp).Sum(n => n.soluong);
            }
            return tongsoluong;
        }
        //Tính tổng tiền
        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tongtien = listGioHang.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        //tạo partial giỏi hàng
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        #endregion

        #region Đặt hàng
        //Xây dựng chức năng đặt hàng
        [HttpPost]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập
            if (Session["TaiKhoanKH"] == null || Session["TaiKhoanKH"].ToString() == "")
            {
                String ten = Request.Form["hoten"].ToString();
                String diachi = Request.Form["diachigiao"].ToString();
                String sdt = Request.Form["sdt"].ToString();
                String email = Request.Form["emails"].ToString();
                //Kiểm tra dơn hàng
                if(ten == "")
                {
                    TempData["loiten"] = "Vui lòng nhập tên!";
                   return RedirectToAction("GioHang", "GioHang");
                }
                if (email == "")
                {
                    TempData["loiemail"] = "Vui lòng nhập Email!";
                    return RedirectToAction("GioHang", "GioHang");
                }
                if (sdt == "")
                {
                    TempData["loisdt"] = "Vui lòng nhập số điện thoại!";
                    return RedirectToAction("GioHang", "GioHang");
                }
                if (diachi == "")
                {
                    TempData["loidiachi"] = "Vui lòng nhập đại chỉ!";
                   return RedirectToAction("GioHang", "GioHang");
                }
                if (Session["GioHang"] == null)
                {
                    RedirectToAction("Index", "CuaHang");
                }
                DONHANG dh = new DONHANG();
                List<GioHang> gh = LayGioHang();
                dh.DCGiao = diachi;
                dh.HoTen = ten;
                dh.SDT = sdt;
                dh.Email = email;
                dh.NgayDat = DateTime.Now;
                dh.TinhTrang = "Chưa duyệt";
                dh.TongTien = (decimal)TongTien();
                db.DONHANGs.Add(dh);
                db.SaveChanges();
                //thêm chi tiết đơn hàng
                foreach (var item in gh)
                {
                    CHITIETHD ctHD = new CHITIETHD();
                    ctHD.MaDH = dh.MaDH;
                    ctHD.MaSP = item.masp;
                    ctHD.MaMau = item.mamau;
                    ctHD.MaSize = item.masize;
                    ctHD.SoLuong = item.soluong;
                    ctHD.DonGia = (decimal)item.dongia;
                    db.CHITIETHDs.Add(ctHD);
                    //string toAddress = kh.Email;
                    //string subject = "Yugo Shoes";
                    //string body = "Cảm ơn bạn đã đặt hàng tại Shop" + "<br/>" + "Mã Hóa Đơn:" + dh.MaDH + "<br/>" + "Tên Sản Phẩm:" + item.tensp + "<br/>" + "Số Lượng:" + item.soluong + "<br/>" + "Đơn Giá:" + item.dongia + "<br/>" + "Thành Tiền:" + item.thanhtien;
                    //SendEmail(toAddress, subject, body);
                }
                db.SaveChanges();
                Session["GioHang"] = null;
            }
            else
            {
                //Kiểm tra dơn hàng
                if (Session["GioHang"] == null)
                {
                    RedirectToAction("Index", "CuaHang");
                }
                //Thêm đơn đặt hàng
                DONHANG dh = new DONHANG();
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoanKH"];
                List<GioHang> gh = LayGioHang();
                dh.MaKH = kh.MAKH;
                dh.DCGiao = kh.DiaChi;
                dh.HoTen = kh.TenKH;
                dh.SDT = kh.Sdt;
                dh.Email = kh.Email;
                dh.NgayDat = DateTime.Now;
                dh.TinhTrang = "Chưa duyệt";
                dh.TongTien = (decimal)TongTien();
                db.DONHANGs.Add(dh);
                db.SaveChanges();
                //thêm chi tiết đơn hàng
                foreach (var item in gh)
                {
                    CHITIETHD ctHD = new CHITIETHD();
                    ctHD.MaDH = dh.MaDH;
                    ctHD.MaSP = item.masp;
                    ctHD.MaMau = item.mamau;
                    ctHD.MaSize = item.masize;
                    ctHD.SoLuong = item.soluong;
                    ctHD.DonGia = (decimal)item.dongia;
                    db.CHITIETHDs.Add(ctHD);
                    //string toAddress = kh.Email;
                    //string subject = "Yugo Shoes";
                    //string body = "Cảm ơn bạn đã đặt hàng tại Shop" + "<br/>" + "Mã Hóa Đơn:" + dh.MaDH + "<br/>" + "Tên Sản Phẩm:" + item.tensp + "<br/>" + "Số Lượng:" + item.soluong + "<br/>" + "Đơn Giá:" + item.dongia + "<br/>" + "Thành Tiền:" + item.thanhtien;
                    //SendEmail(toAddress, subject, body);
                }
                db.SaveChanges();
                Session["GioHang"] = null;
            }
            return RedirectToAction("Index", "CuaHang");
        }
        #endregion

        public void SendEmail(string address, string subject, string message)
        {
            string email = "yugoshoes97@gmail.com";
            string password = "vokykaka9xpro";
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }
}