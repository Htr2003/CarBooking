using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBooking.Models
{
    public class LichSuCuocXeViewModel
    {
        public int DatXeID { get; set; }
        public string TenKhachHang { get; set; }
        public string Sdt { get; set; }
        public DateTime NgayNhanCuoc { get; internal set; }
    }
}