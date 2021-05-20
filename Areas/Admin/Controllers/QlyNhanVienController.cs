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
    public class QlyNhanVienController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlyNhanVien
        public ActionResult DanhSachNhanVien(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<NHANVIEN> listKQ = db.NHANVIENs.Where(n => n.TenNV.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy nhân viên nào phù hợp.";
                    return View(db.NHANVIENs.OrderBy(n => n.TenNV).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.TenNV).ToPagedList(pageNumber, pageSize));
            }
            return View(db.NHANVIENs.OrderBy(n => n.TenNV).ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult ThemMoiNV()
        {
            return View();
        }

        [HttpPost]
        public ViewResult ThemMoiNV(NHANVIEN nv)
        {
            if (ModelState.IsValid)
            {
                //chèn dữ liệu
                db.NHANVIENs.Add(nv);
                //Lưu vào CSDL
                db.SaveChanges();
                TempData["thongbao"] = "Thêm mới nhân viên thành công!";
            }
            else
                TempData["thongbao"] = "Thêm nhân viên thất bại";
            return View();
        }

        [HttpGet]
        public ActionResult ChinhSuaNV(int manv)
        {
            //Lấy ra đối tượng sp theo mã
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(n => n.MaNV == manv);
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nv);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaNV(NHANVIEN nv)
        {
            //Thêm vào CSDL
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong model
                db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["thongbao"] = "Chỉnh sửa thành công!";
            }
            else
                TempData["thongbao"] = "Chỉnh sửa thất bại!!";
            return View(nv);
        }

        //Chi tiết
        public ActionResult HienThiNV(int manv)
        {
            //Lấy ra đối tượng sp theo mã
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(n => n.MaNV == manv);
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nv);
        }
    }
}