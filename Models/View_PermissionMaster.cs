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
    
    public partial class View_PermissionMaster
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserPassword { get; set; }
        public string EmployeeID { get; set; }
        public Nullable<int> PlantID { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string SectionName { get; set; }
        public Nullable<int> UserPermission { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> PageID { get; set; }
        public string PageName { get; set; }
        public string UserEmpID { get; set; }
        public string UserEmail { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
