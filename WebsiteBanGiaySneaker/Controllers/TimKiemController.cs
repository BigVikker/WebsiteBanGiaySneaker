using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteBanGiaySneaker.Controllers
{
    public class TimKiemController : Controller
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: TimKiem
        [HttpPost]
        public ActionResult KQTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> listKQ = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            //Phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (listKQ.Count==0)
            {
                ViewBag.ThongBao1 = "Không tìm thấy sản phẩm nào phù hợp.";
                return View(db.SANPHAMs.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao1 = "Đã tìm thấy " + listKQ.Count + " kết quả!";
            return View(listKQ.OrderBy(n=>n.TenSP).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult KQTimKiem(int? page, string sTuKhoa)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> listKQ = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            //Phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao1 = "Không tìm thấy sản phẩm nào phù hợp.";
                ViewBag.ThongBao2 = "Thử xem một số mẫu giày khác.";
                return View(db.SANPHAMs.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao1 = "Đã tìm thấy " + listKQ.Count + " kết quả!";
            return View(listKQ.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
    }
}