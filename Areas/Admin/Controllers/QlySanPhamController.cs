using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanGiaySneaker.Areas.Admin.Controllers
{
    public class QlySanPhamController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlySanPham
        public ActionResult QuanLySanPham(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<SANPHAM> listKQ = db.SANPHAMs.Where(n => n.TenSP.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["tb"] = "Không tìm thấy sản phẩm nào phù hợp.";
                    return View(db.SANPHAMs.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            }
            return View(db.SANPHAMs.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

       

        [HttpGet]
        public ActionResult ThemMoi()
        {
            ViewBag.TenNSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.TenSP = new SelectList(db.SANPHAMs.ToList().OrderBy(n => n.TenSP), "MaSP", "TenSP");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(SANPHAM sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3)
        {
            int mansx = int.Parse(Request.Form["TenNSX"]);
            ViewBag.TenNSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);

                var fileName2 = Path.GetFileName(fileUpload2.FileName);

                var fileName3 = Path.GetFileName(fileUpload3.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/HinhAnh/HinhAnhSP"), fileName);

                var path2 = Path.Combine(Server.MapPath("~/HinhAnh/HinhAnhSP"), fileName2);

                var path3 = Path.Combine(Server.MapPath("~/HinhAnh/HinhAnhSP"), fileName3);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }

                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path2))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload2.SaveAs(path2);
                }

                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path3))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload3.SaveAs(path3);
                }

                sp.Anh = fileUpload.FileName;
                sp.Anh2 = fileUpload2.FileName;
                sp.Anh3 = fileUpload3.FileName;
                //sp.NgayCapNhat = DateTime.Now;
                sp.MaNSX = mansx;
                db.SANPHAMs.Add(sp);
                db.SaveChanges();
                TempData["thanhcong"] = "Thêm mới sản phẩm thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm sản phẩm thất bại";
            return View();
        }
        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult ChinhSua(int masp)
        {

            //Lấy ra đối tượng sp theo mã
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.TenNSX = new SelectList(db.NSXes.ToList(), "MaNSX", "TenNSX", giay.MaNSX);
            return View(giay);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(SANPHAM giay)
        {
            int mansx = int.Parse(Request.Form["TenNSX"]);
            //Thêm vào CSDL
            if (ModelState.IsValid)
            {

                //Thực hiện cập nhật trong model
                giay.MaNSX = mansx;
                giay.NgayCapNhat = DateTime.Now;
                db.Entry(giay).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["thanhcong"] = "Chỉnh sửa sản phẩm thành công!";
            }
            else
            {
                TempData["kthanhcong"] = "Chỉnh sửa thất bại!";
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.TenNSX = new SelectList(db.NSXes.ToList(), "MaNSX", "TenNSX", giay.MaNSX);
            return View();
        }

        public ActionResult HienThi(int masp)
        {
            //Lấy ra đối tượng sp theo mã
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        //Xóa giày
        [HttpGet]
        public ActionResult Xoa(int masp)
        {
            //Lấy ra đối tượng sp theo mã
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int masp)
        {
            SANPHAM giay = db.SANPHAMs.SingleOrDefault(n => n.MaSP == masp);
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.Remove(giay);
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
        }


        public ActionResult ChiTiet(int masp, int? page)
        {
            TempData["Masp"] = masp;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //Lấy ra đối tượng sp theo mã
            var listgiay = db.CHITIETSPs.Where(n => n.MaSP == masp).OrderBy(n=>n.MaSP).ToPagedList(pageNumber,pageSize);
            if (listgiay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(listgiay);
        }
    }

}