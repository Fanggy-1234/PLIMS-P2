//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PLIMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class View_Services
    {
        public int ServicesID { get; set; }
        public string ServicesName { get; set; }
        public string ServicesRate { get; set; }
        public Nullable<int> ServicesStatus { get; set; }
        public string MonthYear { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string Ser_PlantName { get; set; }
        public string Ser_LineName { get; set; }
    }
}