using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiaySneaker.Models;

namespace WebsiteBanGiaySneaker.Controllers
{
    public class CuaHangController : Controller
    {

        WebsiteBanGiaySneakerEntities db = new WebsiteBanGiaySneakerEntities();
        // GET: CuaHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FooterPartial()
        {
            return PartialView();
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
       
    }
}