﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PhlimsDatabaseEntities : DbContext
    {
        public PhlimsDatabaseEntities()
            : base("name=PhlimsDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TbProduct> TbProducts { get; set; }
        public virtual DbSet<TbSection> TbSections { get; set; }
        public virtual DbSet<TbShift> TbShifts { get; set; }
        public virtual DbSet<TbService> TbServices { get; set; }
        public virtual DbSet<View_Services> View_Services { get; set; }
        public virtual DbSet<TbPlant> TbPlants { get; set; }
        public virtual DbSet<TbIncentiveMaster> TbIncentiveMasters { get; set; }
        public virtual DbSet<TbUser> TbUsers { get; set; }
        public virtual DbSet<TbDefect> TbDefects { get; set; }
        public virtual DbSet<View_Incentive> View_Incentive { get; set; }
        public virtual DbSet<TbReason> TbReasons { get; set; }
        public virtual DbSet<View_Reason> View_Reason { get; set; }
        public virtual DbSet<TbLine> TbLines { get; set; }
        public virtual DbSet<TbEmployeeMaster> TbEmployeeMasters { get; set; }
        public virtual DbSet<View_Employee> View_Employee { get; set; }
        public virtual DbSet<View_PermissionMaster> View_PermissionMaster { get; set; }
        public virtual DbSet<View_User> View_User { get; set; }
        public virtual DbSet<View_EmployeeJoin> View_EmployeeJoin { get; set; }
        public virtual DbSet<TbPLPS> TbPLPS { get; set; }
        public virtual DbSet<View_PLPS> View_PLPS { get; set; }
        public virtual DbSet<TbEmptransaction> TbEmptransactions { get; set; }
        public virtual DbSet<View_EmployeeClocktime> View_EmployeeClocktime { get; set; }
        public virtual DbSet<View_ServicesClocktime> View_ServicesClocktime { get; set; }
        public virtual DbSet<TbPage> TbPages { get; set; }
        public virtual DbSet<TbRole> TbRoles { get; set; }
        public virtual DbSet<TbPermission> TbPermissions { get; set; }
        public virtual DbSet<View_PagePermission> View_PagePermission { get; set; }
    }
}