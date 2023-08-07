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
            {
                

                return RedirectToAction("KeToan", "Users");
            }
                
            else if (VaiTro == "DieuPhoi")
            {
                return RedirectToAction("KeToan", "Users");
            }
                

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

        public ActionResult TaiXe()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DanhSachCuocXe()
        {
            var cuocXeList = database.CT_DATXE.Include("KHACHHANG") .Include("DIADIEM").Include("XE")
                .OrderByDescending(x => x.DatXeID).ToList();

            return View(cuocXeList);
        }

        [HttpPost]
        public ActionResult TiepNhanCuocXe(int rideId)
        {
            var ride = database.CT_DATXE.Find(rideId);

            var txID = (int)Session["taixeID"];
            var driver = database.TAIXEs.Find(txID);

            if (ride != null && ride.TrangThai == "Đang chờ" && driver != null && Session["CurrentRideId"] == null)
            {
                ride.TrangThai = "Đã nhận";
                ride.TaiXeID = driver.TaiXeID;

                Session["NgayNhanCuoc_" + rideId] = DateTime.Now;
                Session["CurrentRideId"] = rideId;
                database.SaveChanges();
            }

            return RedirectToAction("DanhSachCuocXe");
        }

        [HttpPost]
        public ActionResult HoanTatCuocXe(int rideId)
        {
            var ride = database.CT_DATXE.Find(rideId);

            if (ride != null && ride.TrangThai == "Đã nhận" && (int)Session["CurrentRideId"] == rideId)
            {
                ride.TrangThai = "Hoàn tất";
                Session["CurrentRideId"] = null;
                database.SaveChanges();
            }

            return RedirectToAction("DanhSachCuocXe");
        }

        public ActionResult LichSuCuocXe()
        {
            var lichSuCuocXe = database.CT_DATXE
                .Where(x => x.TrangThai == "Hoàn tất").ToList()
                .Select(x => new LichSuCuocXeViewModel
                {
                    DatXeID = (int)x.DatXeID,
                    TenKhachHang = x.KHACHHANG.HoTen,
                    Sdt = x.KHACHHANG.Sdt,
                    NgayNhanCuoc = Session["NgayNhanCuoc_" + x.CT_DatXeID] as DateTime? ?? DateTime.MinValue 
                })
                .ToList();

            return View(lichSuCuocXe);
        }
    }
}