using CarBooking.Models;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System;

namespace CarBooking.Controllers
{
    public class HomeController : Controller
    {
        DBCarBookingEntities database = new DBCarBookingEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTen))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong ho va ten ");

                if (string.IsNullOrEmpty(kh.GioiTinh))
                    ModelState.AddModelError(string.Empty, "gioi tinh kh dc de trong ");

                if (string.IsNullOrEmpty(kh.Sdt))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong sdt");

                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "khong dc de email trong");

                if (string.IsNullOrEmpty(kh.DiaChi))
                    ModelState.AddModelError(string.Empty, "dia chi kh dc de trong");

                if (string.IsNullOrEmpty(kh.TenDangNhap))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong ten dang nhap");

                if (string.IsNullOrEmpty(kh.MatKhau))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong mat khau ");

                if (ModelState.IsValid)
                {
                    database.KHACHHANGs.Add(kh);
                    database.SaveChanges();
                    return RedirectToAction("Login");
                }

                var checkKh = database.KHACHHANGs.FirstOrDefault(k => k.TenDangNhap == kh.TenDangNhap);

                if (checkKh != null)
                {
                    ModelState.AddModelError(string.Empty, "Ten dang nhap da duoc dung, vui long chon ten khac!");
                }
               
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KHACHHANG acc)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(acc.TenDangNhap))
                    ModelState.AddModelError(string.Empty, "Ten dang nhap kh dc de trong");
                if (string.IsNullOrEmpty(acc.MatKhau))
                    ModelState.AddModelError(string.Empty, "mat khau kh dc de trong ");

                var checkTaiXe = database.TAIXEs.FirstOrDefault(t => t.TenDangNhap == acc.TenDangNhap && t.MatKhau == acc.MatKhau);
                if (checkTaiXe != null)
                {
                    Session["TaiXeAccount"] = checkTaiXe.TenDangNhap;
                    return RedirectToAction("Index", "TaiXe");
                }

                var checkNV = database.NHANVIENs.FirstOrDefault(n => n.TenDangNhap == acc.TenDangNhap && n.MatKhau == acc.MatKhau);
                if (checkNV != null)
                {
                    Session["VaiTro"] = checkNV.VaiTro;
                    return RedirectToAction("Index", checkNV.VaiTro);
                }

                var check = database.KHACHHANGs.Where(k => k.TenDangNhap.Equals(acc.TenDangNhap) && k.MatKhau.Equals(acc.MatKhau)).FirstOrDefault();
                if (check != null)
                {
                    Session["KhID"] = check.KhID;
                    Session["Name"] = check.HoTen;
                    Session["TenDangNhap"] = check.TenDangNhap;
                    Session["MatKhau"] = check.MatKhau;
                    Session["TaiKhoan"] = check;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Ten dang nhap hoac mat khau khong dung";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Detail(int KhID = -1)
        {
            if (KhID != -1)
            {
                var info = database.KHACHHANGs.FirstOrDefault(s => s.KhID == KhID);
                return View(info);
            }
            else
            {
                return View();
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormCarBooking()
        {
            return View();  
        }

        [HttpPost]
        public ActionResult FormCarBooking(DIADIEM Ddiem, XE xe, DATXE dx)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Ddiem.DiemXuatPhat))
                    ModelState.AddModelError(string.Empty, "khong duoc de trong diem xuat phat");

                if (string.IsNullOrEmpty(Ddiem.DiemDen))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong diem den");

                if (string.IsNullOrEmpty(xe.LoaiXe))
                    ModelState.AddModelError(string.Empty, "kh dc de trong loai xe");
            }
            if (ModelState.IsValid)
            {
                database.DIADIEMs.Add(Ddiem);

                int diadiemId = Ddiem.DDiemID;

                var datXe = new DATXE
                {
                    XeID = xe.XeID,
                    DDiemID = diadiemId,
                    ThoiGian = dx.ThoiGian 
                };

                database.DATXEs.Add(datXe);

                Session["DiemXuatPhat"] = Ddiem.DiemXuatPhat;
                Session["DiemDen"] = Ddiem.DiemDen;
                Session["ThoiGian"] = dx.ThoiGian;
                Session["DiemXuatPhat"] = xe.LoaiXe;
                Session["DatXe"] = datXe;


                database.SaveChanges();

                return RedirectToAction("BookingInfo");
            }

            else
            {
                ViewBag.thongbao = "moi nhap lai form dat xe";
            }

            return View();
        }

        [HttpGet]
        public ActionResult BookingInfo() { return View(); }

        [HttpPost]
        public ActionResult BookingInfo(KHACHHANG kh, DATXE dx, int id)
        {
            var datXe = database.DATXEs.Include("DIADIEM").FirstOrDefault(d => d.DatXeID == id);

            if (datXe != null)
            {
                return View(datXe);
            }

            return View("Index");
        }
       
    }
}