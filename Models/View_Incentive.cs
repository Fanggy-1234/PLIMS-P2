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
    
    public partial class View_Incentive
    {
        public int IncentiveID { get; set; }
        public string IncentiveName { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string ProductName { get; set; }
        public string SectionName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<decimal> Min { get; set; }
        public Nullable<decimal> Max { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string Grade { get; set; }
    }
}
