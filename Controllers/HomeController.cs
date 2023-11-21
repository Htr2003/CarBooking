using CarBooking.Models;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace CarBooking.Controllers
{
    public class HomeController : Controller
    {
        
        DBCarBookingEntities database = new DBCarBookingEntities();

        private static List<dynamic> TinhGiaTien;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Nếu chưa có dữ liệu, thực hiện đọc dữ liệu
            if (TinhGiaTien == null)
            {
                string jsonFilePath = Server.MapPath("~/App_Data/TinhGiaTien.json");
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                TinhGiaTien = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);
            }
        }
        // Trong constructor hoặc một phương thức khác
        private double TinhTongTien(string quanDi, string quanDen)
        {
            // Tìm giá tiền tương ứng với cặp quận đi và quận đến
            var giaTienInfo = TinhGiaTien.FirstOrDefault(info =>
                info.QuanDi == quanDi && info.QuanDen == quanDen);

            // Nếu tìm thấy thông tin giá tiền, trả về giá tiền; ngược lại, trả về một giá trị mặc định (ví dụ: 0)
            return giaTienInfo != null ? giaTienInfo.GiaTien : 0;
        }

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
                    ModelState.AddModelError(string.Empty, "Mat khau kh dc de trong ");

                var checkTaiXe = database.TAIXEs.FirstOrDefault(t => t.TenDangNhap == acc.TenDangNhap && t.MatKhau == acc.MatKhau);
                if (checkTaiXe != null)
                {
                    Session["taixeID"] = checkTaiXe.TaiXeID;
                    Session["TaiXe"] = checkTaiXe.HoTen;
                    Session["TaiXeAccount"] = checkTaiXe.TenDangNhap;
                    return RedirectToAction("TaiXe", "Users");
                }

                var checkNV = database.NHANVIENs.FirstOrDefault(n => n.TenDangNhap == acc.TenDangNhap && n.MatKhau == acc.MatKhau && n.VaiTro == "DieuPhoi");
                if (checkNV != null)
                {
                    Session["NameNV"] = checkNV.HoTen;
                    Session["NVID"] = checkNV.NhanVienID;
                    Session["VaiTro"] = checkNV.VaiTro;
                    return RedirectToAction("Index", "NVDieuPhoi");
                }

                var check = database.KHACHHANGs.Where(k => k.TenDangNhap.Equals(acc.TenDangNhap) && k.MatKhau.Equals(acc.MatKhau)).FirstOrDefault();
                if (check != null)
                {
                    Session["KhID"] = check.KhID;
                    Session["Name"] = check.HoTen;
                    Session["TenDangNhap"] = check.TenDangNhap;
                    Session["MatKhau"] = check.MatKhau;
                    Session["TaiKhoan"] = check;

                    return RedirectToAction("FormCarBooking", "Home");
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
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult FormCarBooking()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormCarBooking(DIADIEM Ddiem, XE xe, DATXE dx, string QuanDiQuyen, string QuanDenQuyen)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Ddiem.DiemXuatPhat))
                    ModelState.AddModelError(string.Empty, "khong duoc de trong diem xuat phat");

                if (string.IsNullOrEmpty(Ddiem.DiemDen))
                    ModelState.AddModelError(string.Empty, " khong duoc de trong diem den");

                Ddiem.QuanDi = QuanDiQuyen;
                Ddiem.QuanDen = QuanDenQuyen;


                double giaTien = TinhTongTien(QuanDiQuyen, QuanDenQuyen);

                // Gán giá tiền vào đối tượng DIADIEM
                Ddiem.GiaTien = giaTien;

                database.DIADIEMs.Add(Ddiem);
                database.SaveChanges();

                Session["diadiem"] = Ddiem.DDiemID;


                int diadiemId = Ddiem.DDiemID;

                var xeMapping = new Dictionary<string, int>
                {
                    { "xe 4 chỗ", 1 },
                    { "xe 7 chỗ", 2 },
                    { "xe du lịch", 3 }
                };

                if (xeMapping.TryGetValue(xe.LoaiXe, out int xeId))
                {
                    var datXe = new DATXE
                    {
                        DatXeID = dx.DatXeID,
                        XeID = xeId,
                        DDiemID = diadiemId,
                        ThoiGian = dx.ThoiGian
                    };

                    database.DATXEs.Add(datXe);
                    database.SaveChanges();

                    
                    Session["loaixe"] = datXe.XeID;
                    Session["DatXe"] = datXe.DatXeID;

                    return RedirectToAction("BookingInfo");
                }

                else
                {
                    return View("Error", "khong ton tai loai xe nay trong database.");
                }


            }
            else { return ViewBag.thongbao = "moi nhap lai form"; }

        }
        [HttpGet]
        public ActionResult BookingInfo()
        {
            if (Session["DatXe"] == null)
            {
                return RedirectToAction("FormCarBooking");
            }

            var datXeId = (int)Session["DatXe"];
            var d = database.DATXEs.Find(datXeId);

            return View(d);
        }

        [HttpPost]
        public ActionResult BookingInfo(KHACHHANG kh, CT_DATXE cd)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTen))
                    ModelState.AddModelError(string.Empty, "ten khong duoc de trong");
                if (string.IsNullOrEmpty(kh.Sdt))
                    ModelState.AddModelError(string.Empty, "sdt khong duoc de trong");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "email khong duoc de trong");

                if (Session["Name"] != null)
                {
                    var currentKH = (int)Session["KhID"];
                    cd.KhID = currentKH;
                }

                else
                {
                    database.KHACHHANGs.Add(kh);
                    database.SaveChanges();
                    cd.KhID = kh.KhID;
                }

                if (Session["DatXe"] != null && Session["loaixe"] != null && Session["diadiem"] != null)
                {
                    var datXeId = (int)Session["DatXe"];
                    var d = database.DATXEs.Find(datXeId);

                    var xeid = (int)Session["loaixe"];
                    var x = database.XEs.Find(xeid);

                    var ddiemID = (int)Session["diadiem"];
                    var dd = database.DATXEs.Find(ddiemID);

                    
                    cd.DatXeID = d.DatXeID;
                    cd.XeID = x.XeID;
                    cd.DDiemID = dd.DDiemID;
                    cd.TongTien = (int?)dd.DIADIEM.GiaTien;
                    cd.TrangThai = "Đang chờ".ToString();
                }

                database.CT_DATXE.Add(cd);
                database.SaveChanges();

                Session["ctdatxe"] = cd.CT_DatXeID;

                return View("Notify");
            }
            return View();
        }

        public ActionResult LichSu()
        {
            var lichSuCuocXe = database.CT_DATXE.Include("TAIXE").Include("DIADIEM")
                .Include("XE").OrderByDescending(x => x.DatXeID).ToList();

            return View(lichSuCuocXe);
        }

        public ActionResult HuyDatXe()
        {
            var datxeID = (int)Session["ctdatxe"];
            var datxe = database.CT_DATXE.Find(datxeID);

            if (datxe != null && datxe.TaiXeID == null)
            {

                datxe.TrangThai = "Đã hủy".ToString();
                database.SaveChanges();
            }

            return RedirectToAction("LichSu");
        }

        public ActionResult Notify()
        {
            return View();
        }
    }
}