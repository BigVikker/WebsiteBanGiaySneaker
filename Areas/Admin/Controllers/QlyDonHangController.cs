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
    public class QlyDonHangController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlyDonHang
        public ActionResult DanhSachDonHang(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<DONHANG> listKQ = db.DONHANGs.Where(n => n.MaDH.ToString().Contains(timkiem) ||  n.NHANVIEN.TenNV.ToString().Contains(timkiem) ||  n.KHACHHANG.TenKH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.DONHANGs.Where(n => n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONHANGs.Where(n => n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }

        
        public ActionResult DanhSachChuaDuyet(string timkiem, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.TuKhoa = timkiem;
                if (timkiem != null)
                {
                    List<DONHANG> listKQ = db.DONHANGs.Where(n => n.MaDH.ToString().Contains(timkiem) && n.TinhTrang == "Chưa duyệt" || n.NHANVIEN.TenNV.ToString().Contains(timkiem) && n.TinhTrang == "Chưa duyệt" || n.KHACHHANG.TenKH.ToString().Contains(timkiem) && n.TinhTrang == "Chưa duyệt").ToList();
                    if (listKQ.Count == 0)
                    {
                        TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                        return View(db.DONHANGs.Where(n => n.TinhTrang == "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
                    }
                    return View(listKQ.Where(n => n.TinhTrang == "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
                }
            return View(db.DONHANGs.Where(n => n.TinhTrang == "Chưa duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DonHangChoGiao(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<DONHANG> listKQ = db.DONHANGs.Where(n => n.MaDH.ToString().Contains(timkiem) || n.NHANVIEN.TenNV.ToString().Contains(timkiem) || n.KHACHHANG.TenKH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.DONHANGs.Where(n => n.NgayGiao.ToString()=="" && n.TinhTrang=="Đã duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.NgayGiao.ToString() == "" && n.TinhTrang == "Đã duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONHANGs.Where(n => n.NgayGiao.ToString() == "" && n.TinhTrang == "Đã duyệt").OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DuyetDonHang(int madh)
        {
            int manv = (int)Session["MaNV"];
            DONHANG dh = db.DONHANGs.Where(n => n.MaDH == madh).SingleOrDefault();
            dh.MaNV = manv;
            dh.TinhTrang = "Đã duyệt";
            db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachChuaDuyet", "QlyDonHang");
        }

        public ActionResult GiaoHang(int madh)
        {
            int manv = (int)Session["MaNV"];
            DONHANG dh = db.DONHANGs.Where(n => n.MaDH == madh).SingleOrDefault();
            dh.MaNV = manv;
            dh.NgayGiao = DateTime.Now;
            dh.ThanhToan = "Tiền mặt";
            dh.ThanhToan = "Đã thanh toán";
            db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DonHangChoGiao", "QlyDonHang");
        }

        public ActionResult HuyDonHang(int madh)
        {
            int manv = (int)Session["MaNV"];
            List<CHITIETHD> cthd = db.CHITIETHDs.Where(n => n.MaDH == madh).ToList();
            DONHANG dh = db.DONHANGs.Where(n => n.MaDH == madh).SingleOrDefault();
            dh.MaNV = manv;
            dh.TinhTrang = "Đã hủy";
            foreach(var item in cthd)
            {
                db.CHITIETHDs.Remove(item);
                db.SaveChanges();
            }
            db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachDonHang", "QlyDonHang");
        }

        public ActionResult ChiTietDH(int madh, string timkiem, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.TuKhoa = timkiem;
            if (timkiem != null)
            {
                List<CHITIETHD> listKQ = db.CHITIETHDs.Where(n => n.MaDH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.CHITIETHDs.Where(n => n.MaDH==madh).OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaDH==madh).ToPagedList(pageNumber, pageSize));
            }
            return View(db.CHITIETHDs.Where(n=>n.MaDH==madh).OrderByDescending(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }
    }
}