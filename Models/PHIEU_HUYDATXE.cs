//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarBooking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PHIEU_HUYDATXE
    {
        public int PhieuID { get; set; }
        public Nullable<int> KhID { get; set; }
        public Nullable<int> DatXeID { get; set; }
        public Nullable<System.DateTime> ThoiGianHuy { get; set; }
        public string LyDo { get; set; }
    
        public virtual DATXE DATXE { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}
