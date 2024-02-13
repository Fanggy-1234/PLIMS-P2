using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using PLIMS.Models;

namespace PLIMS.ViewModel
{
    public class ViewModelAll
    {

        public IEnumerable<TbPlant> tbPlants { get; set; }
        public IEnumerable<TbLine> tbLine { get; set; }
        public IEnumerable<TbProduct> tbProduct { get; set; }
        public IEnumerable<TbService> tbService { get; set; }
        public IEnumerable<TbSection> tbSection { get; set; }
        public IEnumerable<TbIncentiveMaster> tbIncentiveMaster { get; set; }

        public IEnumerable<TbEmployeeMaster> tbEmployeeMaster { get; set; }

        public IEnumerable<TbUser> tbUser { get; set; }

        public IEnumerable<TbShift> tbShift { get; set; }
        public IEnumerable<TbEmptransaction> tbEmptransaction { get; set; }

        public IEnumerable<TbPermission> tbPermission { get; set; }
        public IEnumerable<TbRole> tbRole { get; set; }

        public IEnumerable<TbReason> tbReason { get; set; }

        public IEnumerable<TbPLPS> tbPLPS { get; set; }

        public IEnumerable<TbPage> tbPage { get; set; }



        public IEnumerable<View_User> view_User { get; set; }
        public IEnumerable<View_Employee> view_Employee { get; set; }
        public IEnumerable<View_Incentive> View_Incentives { get; set; }
        public IEnumerable<View_PLPS> view_PLPS { get; set; }

        public IEnumerable<View_EmployeeClocktime> view_EmployeeClocktime { get; set; }
        public IEnumerable<View_ServicesClocktime> view_ServicesClocktime { get; set; }
        public IEnumerable<View_Reason> view_Reason { get; set; }
        public IEnumerable<View_PermissionMaster> view_PermissionMaster { get; set; }
        public IEnumerable<View_Services> view_Services { get; set; }

        public List<View_PagePermission> view_PagePermission { get; set; }

  

        //public string LineNames { get; set; }
        //public string EmployeeIDs { get; set; }


    }
}