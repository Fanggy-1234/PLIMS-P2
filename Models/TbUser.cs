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
    
    public partial class TbUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserPassword { get; set; }
        public Nullable<int> UserPermission { get; set; }
        public string UserEmpID { get; set; }
        public Nullable<int> Status { get; set; }
        public string UserEmail { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> PasswordLastUpdate { get; set; }
        public string Lineconcern { get; set; }
    }
}
