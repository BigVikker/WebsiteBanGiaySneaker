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
    public class QlyKhachHangController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlyKhachHang
        public ActionResult DanhSachKhachHang(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<KHACHHANG> listKQ = db.KHACHHANGs.Where(n => n.TenKH.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy khách hàng nào phù hợp.";
                    return View(db.KHACHHANGs.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));
            }
            return View(db.KHACHHANGs.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));
        }

        //Chi tiết
        public ActionResult HienThiKH(int makh)
        {
            //Lấy ra đối tượng sp theo mã
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
    }
}