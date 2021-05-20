using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteBanGiaySneaker.Areas.Admin.Controllers
{
   
    public class QlyNhapHangController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlyNhapHang
        public ActionResult DanhSachPhieuNhap(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<PHIEUNHAP> listKQ = db.PHIEUNHAPs.Where(n => n.MaPN.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thanhcong"] = "Không tìm thấy phiếu nhập nào phù hợp.";
                    return View(db.PHIEUNHAPs.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
            }
            return View(db.PHIEUNHAPs.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietPN(int mapn, string timkiem, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.TuKhoa = timkiem;
            TempData["MaPN"] = mapn;
            if (timkiem != null)
            {
                List<CHITIETPN> listKQ = db.CHITIETPNs.Where(n => n.MaPN.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.CHITIETPNs.Where(n => n.MaPN == mapn).OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaPN == mapn).ToPagedList(pageNumber, pageSize));
            }
            return View(db.CHITIETPNs.Where(n => n.MaPN == mapn).OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemMoiPN()
        {
            ViewBag.TenNCC = new SelectList(db.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiPN(PHIEUNHAP pn)
        {
            int mancc = int.Parse(Request.Form["TenNCC"]);
            ViewBag.TenNCC = new SelectList(db.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            int manv = (int)Session["MaNV"];
            if (ModelState.IsValid)
            {
                //chèn dữ liệu
                pn.MaNCC = mancc;
                pn.MaNV = manv;
                db.PHIEUNHAPs.Add(pn);
                //Lưu vào CSDL
                db.SaveChanges();
                TempData["thanhcong"] = "Thêm phiếu nhập thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm phiếu nhập thất bại";
            return View();
        }

        [HttpGet]
        public ActionResult ThemMoiCTPN(int mapn)
        {
            ViewBag.TenSP = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSP), "MaSP", "TenSP");
            ViewBag.TenMau = new SelectList(db.MauSacs.ToList().OrderBy(n => n.Color), "MaMau", "Color");
            ViewBag.TenSize = new SelectList(db.Sizes.ToList().OrderBy(n => n.Size1), "MaSize", "Size1");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiCTPN(CHITIETPN ctpn)
        {
            TempData["MaPN"] = ctpn;
            int masp = int.Parse(Request.Form["TenSP"]);
            int mamau = int.Parse(Request.Form["TenMau"]);  
            int masize = int.Parse(Request.Form["TenSize"]);
            int soluong = int.Parse(Request.Form["SoLuong"]);
            int gia = int.Parse(Request.Form["Gia"]);
            ViewBag.TenSP = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSP), "MaSP", "TenSP");
            ViewBag.TenMau = new SelectList(db.MauSacs.ToList().OrderBy(n => n.Color), "MaMau", "Color");
            ViewBag.TenSize = new SelectList(db.Sizes.ToList().OrderBy(n => n.Size1), "MaSize", "Size1");
            //var ctpnkt = db.CHITIETPNs.Where(n => n.MaSP == masp && n.MaSize == masize && n.MaMau == mamau).Count();
            CHITIETSP ctsp = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == masp && n.MaSize == masize && n.MaMau == mamau);
            CHITIETSP ctspn = new CHITIETSP();
            //if (ctpnkt > 0)
            //{
            //    TempData["loi"] = "Sản phẩm trong phiếu nhập tồn tại";
            //    ModelState.AddModelError("loi", " ");
            //    return RedirectToAction("ThemMoiCTPN");
            //}
            if (ModelState.IsValid)
            {
                if(ctsp==null)
                {
                    ctspn.MaSP = masp;
                    ctspn.MaMau = mamau;
                    ctspn.MaSize = masize;
                    ctspn.SoLuong = soluong;
                    db.CHITIETSPs.Add(ctspn);
                    db.SaveChanges();
                }              
                //chèn dữ liệu
                ctpn.MaSP = masp;
                ctpn.MaMau = mamau;
                ctpn.MaSize = masize;
                ctpn.ThanhTien = soluong * gia;
                db.CHITIETPNs.Add(ctpn);
                //Lưu vào CSDL
                db.SaveChanges();
                TempData["thanhtien"] = ctpn.ThanhTien;
                TempData["thanhcong"] = "Thêm thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm thất bại";
            return View();
        }

        public ActionResult HuyPhieuNhap(int mapn)
        {
            int manv = (int)Session["MaNV"];
            List<CHITIETPN> ctpn = db.CHITIETPNs.Where(n => n.MaPN == mapn).ToList();
            PHIEUNHAP pn = db.PHIEUNHAPs.Where(n => n.MaPN == mapn).SingleOrDefault();
            pn.MaNV = manv;
            pn.TinhTrang = "Đã hủy";
            foreach (var item in ctpn)
            {
                db.CHITIETPNs.Remove(item);
                db.SaveChanges();
            }
            db.Entry(pn).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachPhieuNhap", "QlyNhapHang");
        }
    }
}