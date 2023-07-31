using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBooking.Models
{
    public class CartItem
    {
        DBCarBookingEntities db = new DBCarBookingEntities();
        public int DatXeID { get; set; }
        public string DiemXuatPhat { get; set; }
        public string DiemDen { get; set; }
        public string LoaiXe { get ; set; }
        public DateTime ThoiGian { get; set; }

        public CartItem(int DatXeID)
        {
            this.DatXeID = DatXeID;
            var datxe = db.DATXEs.Single(d => d.DatXeID == this.DatXeID);

            DiemXuatPhat = datxe.DIADIEM.DiemXuatPhat;
            DiemDen = datxe.DIADIEM.DiemDen;
            LoaiXe = datxe.XE.LoaiXe;
            ThoiGian = (DateTime)datxe.ThoiGian;
        } 
    }
}