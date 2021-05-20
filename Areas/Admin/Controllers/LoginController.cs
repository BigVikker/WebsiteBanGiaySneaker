using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebsiteBanGiaySneaker.Models;

namespace WebsiteBanGiaySneaker.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult kCoQuyen()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string sEmail = f["txtemail"].ToString();
            string sMatkhau = f["txtmatkhau"].ToString();
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(n => n.Email == sEmail && n.MatKhau == sMatkhau);
            if(nv!=null)
            {
                if (nv.QuyenNV == "1")
                {
                    ViewBag.ThongBao = "Xin chào, Admin:" + nv.TenNV;
                    FormsAuthentication.SetAuthCookie(nv.TenNV, false);
                    Session["TaiKhoanNV"] = nv;
                    Session["User"] = nv.TenNV;
                    Session["Pw"] = nv.MatKhau;
                    Session["MaNV"] = nv.MaNV;
                    Session["Quyen"] = nv.QuyenNV;
                    //TempData["thongbao"] = "Chào mừng bạn đến với trang quản lý của Admin";
                    return RedirectToAction("Index", "ThongKe");
                }
                else
                {
                    ViewBag.ThongBao = "Xin chào " + nv.TenNV;
                    FormsAuthentication.SetAuthCookie(nv.TenNV, false);
                    Session["TaiKhoanNV"] = nv;
                    Session["User"] = nv.TenNV;
                    Session["Pw"] = nv.MatKhau;
                    Session["Quyen"] = nv.QuyenNV;
                    //TempData["thongbao"] = "Chào mừng bạn đến với trang quản lý của nhân viên";
                    return RedirectToAction("Index", "ThongKe");
                }
            }
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!!!";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}