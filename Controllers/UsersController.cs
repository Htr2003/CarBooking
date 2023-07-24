using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarBooking.Models;

namespace CarBooking.Controllers
{
    public class UsersController : Controller
    {
        
        private DBCarBookingEntities database = new DBCarBookingEntities();
        // GET: Users

        public ActionResult Index()
        {
            var VaiTro = Session["VaiTro"]?.ToString();

            if (VaiTro == "KeToan")
                return RedirectToAction("Index", "KeToan");
            else if (VaiTro == "DieuPhoi")
                return RedirectToAction("Index", "NVDieuPhoi");

            else
            {
                // Unknown role or unauthorized access
                return RedirectToAction("Unauthorized", "Error");
            }
        }

        public ActionResult KeToan()
        {
            return View();
        }

        public ActionResult NVDieuPhoi()
        {
            return View();
        }
    }
}