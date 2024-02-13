using PLIMS.Models;
using PLIMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLIMS.Controllers
{
    public class WorkingController : Controller
    {

        PhlimsDatabaseEntities db = new PhlimsDatabaseEntities();
        // GET: Working
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WorkingFunction()
        {
            var EmpRecord = new ViewModelAll()
            {
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                tbSection = db.TbSections.ToList(),
                tbShift = db.TbShifts.ToList(),
                tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                view_Employee = db.View_Employee.ToList(),
                tbProduct = db.TbProducts.ToList()

            };
            return View(EmpRecord);
        }







    }
}