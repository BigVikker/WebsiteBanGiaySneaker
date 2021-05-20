
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;

namespace WebsiteBanGiaySneaker.Areas.Admin.Controllers
{
    public class ThongKeController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/ThongKe
        public ActionResult Index(DateTime? NgayA,DateTime? NgayB)
        {
            //Tính tổng doanh thu
            TempData["TongDoanhThu"] = db.DONHANGs.Where(n => n.TinhTrang == "Đã duyệt" && n.NgayGiao.ToString() != "").Sum(n=>n.TongTien);

            //Đếm đơn hàng chưa duyệt
            TempData["DonHangChuaDuyet"] = db.DONHANGs.Where(n => n.TinhTrang == "Chưa duyệt").Count();

            //Đếm đơn hàng chờ giao
            TempData["DonHangChoGiao"] = db.DONHANGs.Where(n => n.TinhTrang == "Đã duyệt" && n.NgayGiao.ToString() == "").Count();

            //Đếm số khách hàng
            TempData["TongKhachHang"] = db.KHACHHANGs.Count();
            return View(db.DONHANGs.Where(n => n.NgayDat >= NgayA && n.NgayDat < NgayB && n.TinhTrang == "Đã duyệt").ToList());
        }


        public Object TKDoanhThuTheoNgay(DateTime? NgayA,DateTime? NgayB)
        {
            TempData["NgayA"]= NgayA;
            TempData["NgayB"] = NgayB;
            if(NgayA==null||NgayB==null)
            {
                 TempData["DoanhThuTheoNgay"] = "0";
                return RedirectToAction("Index", "ThongKe");
            }
           TempData["DoanhThuTheoNgay"] = db.DONHANGs.Where(n=>n.NgayGiao > NgayA && n.NgayGiao <= NgayB && n.TinhTrang=="Đã duyệt" && n.ThanhToan == "Đã thanh toán").Sum(n=>n.TongTien);
            return RedirectToAction("Index", "ThongKe");
        } 
    }
}