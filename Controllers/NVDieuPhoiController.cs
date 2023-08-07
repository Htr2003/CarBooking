using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarBooking.Models;

namespace CarBooking.Controllers
{
    public class NVDieuPhoiController : Controller
    {
        private DBCarBookingEntities db = new DBCarBookingEntities();

        // GET: NVDieuPhoi
        public ActionResult Index()
        {
            var cT_DATXE = db.CT_DATXE.Include(c => c.DATXE).Include(c => c.DIADIEM).Include(c => c.KHACHHANG).Include(c => c.DICHVU).Include(c => c.TAIXE).Include(c => c.XE);
            return View(cT_DATXE.ToList());
        }

        // GET: NVDieuPhoi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_DATXE cT_DATXE = db.CT_DATXE.Find(id);
            if (cT_DATXE == null)
            {
                return HttpNotFound();
            }
            return View(cT_DATXE);
        }

        // GET: NVDieuPhoi/Create
        public ActionResult Create()
        {
            ViewBag.DatXeID = new SelectList(db.DATXEs, "DatXeID", "DatXeID");
            ViewBag.DDiemID = new SelectList(db.DIADIEMs, "DDiemID", "DiemXuatPhat");
            ViewBag.KhID = new SelectList(db.KHACHHANGs, "KhID", "HoTen");
            ViewBag.DVuID = new SelectList(db.DICHVUs, "DVuID", "TenDV");
            ViewBag.TaiXeID = new SelectList(db.TAIXEs, "TaiXeID", "HoTen");
            ViewBag.XeID = new SelectList(db.XEs, "XeID", "TenXe");
            return View();
        }

        // POST: NVDieuPhoi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CT_DatXeID,DatXeID,XeID,TaiXeID,DDiemID,KhID,DVuID,TongTien,GhiChu,TrangThai")] CT_DATXE cT_DATXE)
        {
            if (ModelState.IsValid)
            {
                db.CT_DATXE.Add(cT_DATXE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DatXeID = new SelectList(db.DATXEs, "DatXeID", "DatXeID", cT_DATXE.DatXeID);
            ViewBag.DDiemID = new SelectList(db.DIADIEMs, "DDiemID", "DiemXuatPhat", cT_DATXE.DDiemID);
            ViewBag.KhID = new SelectList(db.KHACHHANGs, "KhID", "HoTen", cT_DATXE.KhID);
            ViewBag.DVuID = new SelectList(db.DICHVUs, "DVuID", "TenDV", cT_DATXE.DVuID);
            ViewBag.TaiXeID = new SelectList(db.TAIXEs, "TaiXeID", "HoTen", cT_DATXE.TaiXeID);
            ViewBag.XeID = new SelectList(db.XEs, "XeID", "TenXe", cT_DATXE.XeID);
            return View(cT_DATXE);
        }

        // GET: NVDieuPhoi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_DATXE cT_DATXE = db.CT_DATXE.Find(id);
            if (cT_DATXE == null)
            {
                return HttpNotFound();
            }
            ViewBag.DatXeID = new SelectList(db.DATXEs, "DatXeID", "DatXeID", cT_DATXE.DatXeID);
            ViewBag.DDiemID = new SelectList(db.DIADIEMs, "DDiemID", "DiemXuatPhat", cT_DATXE.DDiemID);
            ViewBag.KhID = new SelectList(db.KHACHHANGs, "KhID", "HoTen", cT_DATXE.KhID);
            ViewBag.DVuID = new SelectList(db.DICHVUs, "DVuID", "TenDV", cT_DATXE.DVuID);
            ViewBag.TaiXeID = new SelectList(db.TAIXEs, "TaiXeID", "HoTen", cT_DATXE.TaiXeID);
            ViewBag.XeID = new SelectList(db.XEs, "XeID", "TenXe", cT_DATXE.XeID);
            return View(cT_DATXE);
        }

        // POST: NVDieuPhoi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CT_DatXeID,DatXeID,XeID,TaiXeID,DDiemID,KhID,DVuID,TongTien,GhiChu,TrangThai")] CT_DATXE cT_DATXE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cT_DATXE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DatXeID = new SelectList(db.DATXEs, "DatXeID", "DatXeID", cT_DATXE.DatXeID);
            ViewBag.DDiemID = new SelectList(db.DIADIEMs, "DDiemID", "DiemXuatPhat", cT_DATXE.DDiemID);
            ViewBag.KhID = new SelectList(db.KHACHHANGs, "KhID", "HoTen", cT_DATXE.KhID);
            ViewBag.DVuID = new SelectList(db.DICHVUs, "DVuID", "TenDV", cT_DATXE.DVuID);
            ViewBag.TaiXeID = new SelectList(db.TAIXEs, "TaiXeID", "HoTen", cT_DATXE.TaiXeID);
            ViewBag.XeID = new SelectList(db.XEs, "XeID", "TenXe", cT_DATXE.XeID);
            return View(cT_DATXE);
        }

        // GET: NVDieuPhoi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CT_DATXE cT_DATXE = db.CT_DATXE.Find(id);
            if (cT_DATXE == null)
            {
                return HttpNotFound();
            }
            return View(cT_DATXE);
        }

        // POST: NVDieuPhoi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CT_DATXE cT_DATXE = db.CT_DATXE.Find(id);
            db.CT_DATXE.Remove(cT_DATXE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
