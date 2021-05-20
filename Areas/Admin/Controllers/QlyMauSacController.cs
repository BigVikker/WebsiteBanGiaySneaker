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
    public class QlyMauSacController : BaseController
    {
        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: Admin/QlyMauSac
        public ActionResult DanhSachMauSac(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            if (timkiem != null)
            {
                List<MauSac> listKQ = db.MauSacs.Where(n => n.Color.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy màu nào phù hợp.";
                    return View(db.MauSacs.OrderBy(n => n.MaMau).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.MaMau).ToPagedList(pageNumber, pageSize));
            }
            return View(db.MauSacs.OrderBy(n => n.MaMau).ToPagedList(pageNumber, pageSize));
        }


        // GET: Admin/QlyMauSac/Create
        public ActionResult ThemMoi()
        {
            return View();
        }

        // POST: Admin/QlyMauSac/Create
        [HttpPost]
        public ActionResult ThemMoi(MauSac mau)
        {
            if (ModelState.IsValid)
            {
                //chèn dữ liệu
                db.MauSacs.Add(mau);
                //Lưu vào CSDL
                db.SaveChanges();
                TempData["thongbao"] = "Thêm mới màu thành công!";
            }
            else
                TempData["thongbao"] = "Thêm mới màu thất bại";
            return View();
        }

        // GET: Admin/QlyMauSac/Edit/5
        public ActionResult ChinhSuaMau(int mamau)
        {
            MauSac mau = db.MauSacs.SingleOrDefault(n => n.MaMau == mamau);
            return View(mau);
        }

        // POST: Admin/QlyMauSac/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaMau(MauSac mau)
        {
            //Thêm vào CSDL
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong model
                db.Entry(mau).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["thongbao"] = "Chỉnh sửa thành công!";
            }
            else
                TempData["thongbao"] = "Chỉnh sửa thất bại!!";
            return View(mau);
        }

    }
}
