using OfficeOpenXml;
using PLIMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using System.IO;
using PLIMS.ViewModel;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Channels;
using OfficeOpenXml.ThreadedComments;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Media.Animation;

namespace PLIMS.Controllers
{
    public class EmployeeController : Controller
    {
        PhlimsDatabaseEntities db = new PhlimsDatabaseEntities();
        // GET: Employee
        public ActionResult UserInformation()
        {
            //List<TbEmployeeMaster> tbEmployeeMasters = db.TbEmployeeMasters.ToList();
            //List<TbUser> tbUsers = db.TbUsers.ToList();
            //var UserRecord = from tbe in tbEmployeeMasters
            //                 join tbu in tbUsers on tbe.EmployeeID equals tbu.UserEmpID into t1
            //                 select t1;
            return View();
        }


    

            public ActionResult EmployeeManagement()

        { 
            var EmpRecord =  new ViewModelAll()
                            {
                                tbPlants = db.TbPlants.ToList(),
                                tbLine = db.TbLines.ToList(),
                                tbSection = db.TbSections.ToList(),
                                tbShift = db.TbShifts.ToList(),
                                tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                                 view_Employee = db.View_Employee.ToList()

            };
            return View(EmpRecord);


        }



        [HttpGet]
        public JsonResult EmployeeManagementEdit(int ID)
        {
            var Emps = db.View_Employee.Where(x => x.ID.Equals(ID)).SingleOrDefault();
            return Json(Emps, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateQR(string EmployeeID)
        {


            string data = EmployeeID;

            // Generate the QR code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);


            // Specify the folder path to save the QR code image
            string folderPath = @"D:\QRCODE";

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save the QR code as a PNG image file inside the specified folder
            string fileName = Path.Combine(folderPath, data + "_" + "QRCode.png");
            qrCodeImage.Save(fileName, ImageFormat.Png);

            // Display the QR code image using an image viewer application
            DisplayQRCodeImage(fileName);


            //Update QR Status
            var Empdb = db.TbEmployeeMasters.Where(x => x.EmployeeID == EmployeeID).SingleOrDefault();
            Empdb.UpdateBy = User.Identity.Name;
            Empdb.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("EmployeeManagement");
        }



        static void DisplayQRCodeImage(string imagePath)
        {
            try
            {
                // Check if the file exists
                if (System.IO.File.Exists(imagePath))
                {
                    // Use the default image viewer to open and display the QR code image
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = imagePath,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    Console.WriteLine("QR code image not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }







        /// <summary>
        /// Employee Clock In function
        /// </summary>
        public ActionResult EmployeeClockIn(View_EmployeeClocktime obj, string[] EmployeeID)
        {

            
            if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
            {

                
                var Employee = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                var ViewEmp = Employee.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName.Trim())).ToList();
                    Employee.view_Employee = ViewEmp;

                return View(Employee);

            }
            else
            {

                var Employee = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                return View(Employee);
            }
            
            }



        [HttpGet]
        public ActionResult EmployeeClockIn(FormCollection form,View_EmployeeClocktime obj,string[] EmployeeID)
        {

            if (EmployeeID == null)
            {


                if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
                {


                    var Employee = new ViewModelAll
                    {
                        tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                        tbLine = db.TbLines.ToList(),
                        tbSection = db.TbSections.ToList(),
                        tbService = db.TbServices.ToList(),
                        view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                        tbShift = db.TbShifts.ToList(),
                        tbEmptransaction = db.TbEmptransactions.ToList(),

                    };

                    var ViewEmp = Employee.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName.Trim())).ToList();
                    Employee.view_Employee = ViewEmp;

                    return View(Employee);

                }
                else
                {

                    var Employee = new ViewModelAll
                    {
                        tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                        tbLine = db.TbLines.ToList(),
                        tbSection = db.TbSections.ToList(),
                        tbService = db.TbServices.ToList(),
                        view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                        tbShift = db.TbShifts.ToList(),
                        tbEmptransaction = db.TbEmptransactions.ToList(),

                    };

                    return View(Employee);
                }

            }
            else
            {
                // Create Function
                int datacnt = EmployeeID.Count();
                for (int i = 1; i < datacnt; ++i)
                {

                    // 1. check tbEmpTransaction == Null ?
                    var Empdb = new TbEmptransaction();
                    //var Emp = db.TbEmployeeMasters.Where(x => x.EmployeeID.Equals(EmployeeID[i]));
                    string empid = EmployeeID[i];
                    var EmpTran = db.TbEmptransactions.Where(x => x.EmployeeID.Equals(empid) && x.TransactionDate == obj.TransactionDate).ToList();
                    if (EmpTran.Count() != 0)
                    {
                        //Update Transaction
                        Empdb = db.TbEmptransactions.Where(x => x.EmployeeID == EmployeeID[i] && x.TransactionDate == obj.TransactionDate).SingleOrDefault();
                        Empdb.ClockIn = obj.ClockIn.ToString();
                        Empdb.UpdateBy = User.Identity.Name;
                        Empdb.UpdateDate = DateTime.Now;
                        db.SaveChanges();
                        return RedirectToAction("EmployeeClockIn");
                    }
                    else
                    {
                        string workst;

                        if (obj.WorkingStatus == null)
                        { workst = "Working"; }
                        else
                        { workst = obj.WorkingStatus; }
                        var empdetails = db.TbEmployeeMasters.Where(x => x.EmployeeID == empid.Trim()).SingleOrDefault();
                        // Create Transaction
                        // Insert new Line               
                        if (!string.IsNullOrEmpty(obj.ClockOut))
                        {
                            db.TbEmptransactions.Add(new TbEmptransaction()
                            {
                                TransactionNo = db.TbEmptransactions.Count() + 1,
                                TransactionDate = obj.TransactionDate,
                                EmployeeID = empid,
                                Shift = empdetails.ShiftID,
                                Line = empdetails.LineID,//obj.LineName,
                                WorkingStatus = workst,
                                ClockIn = obj.ClockOut,
                                ClockOut = "",
                                CreateDate = DateTime.Now,
                                CreateBy = User.Identity.Name,
                                UpdateDate = DateTime.Now,
                                UpdateBy = User.Identity.Name,
                            }); ;


                        }
                        else
                        {

                            db.TbEmptransactions.Add(new TbEmptransaction()
                            {
                                TransactionNo = db.TbEmptransactions.Count() + 1,
                                TransactionDate = obj.TransactionDate,
                                EmployeeID = empid,
                                Shift = empdetails.ShiftID,
                                Line = empdetails.LineID,//obj.LineName,
                                ClockIn = obj.ClockIn,
                                CreateDate = DateTime.Now,
                                CreateBy = User.Identity.Name,
                                UpdateDate = DateTime.Now,
                                UpdateBy = User.Identity.Name,
                            });



                        }
                        db.SaveChanges();

                    }
                }
                return RedirectToAction("EmployeeClockIn");

            }

        }




        //3.  Function Employee Clock in Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult EmployeeClockInEdit(int id)
        {
      
            var Emps = db.View_Employee.Where(x => x.ID.Equals(id)).SingleOrDefault();
            return Json(Emps, JsonRequestBehavior.AllowGet);
        }


        //4.  Function Employee Clock In Update Transaction
        [HttpPost]
        public ActionResult EmployeeClockInUpdate(TbEmptransaction obj)
        {

     
            var Empdb = new TbEmptransaction();
         
            var EmpTran = db.TbEmptransactions.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.TransactionDate== DateTime.Today.Date).ToList();
            if (EmpTran.Count() != 0)
            {
                //Update Transaction
                Empdb = db.TbEmptransactions.Where(x => x.EmployeeID == obj.EmployeeID && x.TransactionDate == DateTime.Today.Date).SingleOrDefault();
                Empdb.ClockIn = obj.ClockIn;
                Empdb.UpdateBy = User.Identity.Name;
                Empdb.UpdateDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("EmployeeClockIn");
            }
            else
            {
                string workst ; 
                
                if (obj.WorkingStatus == null)
                { workst = "Working"; } 
                else 
                { workst = obj.WorkingStatus; }
                var empdetails = db.TbEmployeeMasters.Where(x => x.EmployeeID == obj.EmployeeID.Trim()).SingleOrDefault();
                // Create Transaction
                // Insert new Line               
                db.TbEmptransactions.Add(new TbEmptransaction()
                {
                    TransactionNo = db.TbEmptransactions.Count() + 1,
                    TransactionDate = DateTime.Today.Date,
                    EmployeeID = obj.EmployeeID,
                    Shift = 1,
                    Line = obj.Line,
                    Section = obj.Section,
                    WorkingStatus = workst,
                    ClockIn = obj.ClockIn,
                    CreateDate = DateTime.Now,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Now,
                    UpdateBy = User.Identity.Name,
                }) ; 
                db.SaveChanges();

            }
      
            return RedirectToAction("EmployeeClockIn");

        }

        // 5. Function Employee Clock In Update Create Transaction
        [HttpPost]
        public ActionResult EmployeeClockInCreate(View_EmployeeClocktime obj)
        {


            var Empdb = new TbEmptransaction();

            var EmpTran = db.TbEmptransactions.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.TransactionDate == DateTime.Today.Date).ToList();
            if (EmpTran.Count() != 0)
            {
                //Update Transaction
                Empdb = db.TbEmptransactions.Where(x => x.EmployeeID == obj.EmployeeID && x.TransactionDate == DateTime.Today.Date).SingleOrDefault();
                Empdb.ClockIn = obj.ClockIn.ToString();
                Empdb.UpdateBy = User.Identity.Name;
                Empdb.UpdateDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("EmployeeClockIn");
            }
            else
            {
                string workst;

                if (obj.WorkingStatus == null)
                { workst = "Working"; }
                else
                { workst = obj.WorkingStatus; }
                var empdetails = db.TbEmployeeMasters.Where(x => x.EmployeeID == obj.EmployeeID.Trim()).SingleOrDefault();
                // Create Transaction
                // Insert new Line               
                db.TbEmptransactions.Add(new TbEmptransaction()
                {
                    TransactionNo = db.TbEmptransactions.Count() + 1,
                    TransactionDate = DateTime.Today.Date,
                    EmployeeID = obj.EmployeeID,
                    Shift = 1,
                    Line = 1,//obj.Line,
                    WorkingStatus = workst,
                    ClockIn = obj.ClockIn.ToString(),
                    CreateDate = DateTime.Now,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Now,
                    UpdateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }

            return RedirectToAction("EmployeeClockIn");


        }


        // End Employee Clock in Function
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////




        /// <summary>
        /// Employee Clock out Function
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeClocKOut(View_EmployeeClocktime obj, string[] EmployeeID)
        {


            if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
            {


                var Employee = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                var ViewEmp = Employee.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName.Trim())).ToList();
                Employee.view_Employee = ViewEmp;

                return View(Employee);

            }
            else
            {

                var Employee = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                return View(Employee);
            }

        }



        [HttpGet]
        public ActionResult EmployeeClockOut(FormCollection form, View_EmployeeClocktime obj, string[] EmployeeID)
        {

            if (EmployeeID == null)
            {


                if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
                {


                    var Employee = new ViewModelAll
                    {
                        tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                        tbLine = db.TbLines.ToList(),
                        tbSection = db.TbSections.ToList(),
                        tbService = db.TbServices.ToList(),
                        view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                        tbShift = db.TbShifts.ToList(),
                        tbEmptransaction = db.TbEmptransactions.ToList(),

                    };

                    var ViewEmp = Employee.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName.Trim())).ToList();
                    Employee.view_Employee = ViewEmp;

                    return View(Employee);

                }
                else
                {

                    var Employee = new ViewModelAll
                    {
                        tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                        tbLine = db.TbLines.ToList(),
                        tbSection = db.TbSections.ToList(),
                        tbService = db.TbServices.ToList(),
                        view_EmployeeClocktime = db.View_EmployeeClocktime.ToList(),
                        tbShift = db.TbShifts.ToList(),
                        tbEmptransaction = db.TbEmptransactions.ToList(),

                    };

                    return View(Employee);
                }

            }
            else
            {
                // Create Function
                int datacnt = EmployeeID.Count();
                for (int i = 1; i < datacnt; ++i)
                {

                    // 1. check tbEmpTransaction == Null ?
                    var Empdb = new TbEmptransaction();
                    //var Emp = db.TbEmployeeMasters.Where(x => x.EmployeeID.Equals(EmployeeID[i]));
                    string empid = EmployeeID[i];
                    var EmpTran = db.TbEmptransactions.Where(x => x.EmployeeID.Equals(empid) && x.TransactionDate == obj.TransactionDate).ToList();
                    if (EmpTran.Count() != 0)
                    {
                        //Update Transaction
                        Empdb = db.TbEmptransactions.Where(x => x.EmployeeID == empid && x.TransactionDate == obj.TransactionDate).SingleOrDefault();
                        Empdb.ClockOut = obj.ClockOut;
                        Empdb.UpdateBy = User.Identity.Name;
                        Empdb.UpdateDate = DateTime.Now;
                        db.SaveChanges();
                        return RedirectToAction("EmployeeClockOut");
                    }
                    else
                    {
                        string workst;

                        if (obj.WorkingStatus == null)
                        { workst = "Working"; }
                        else
                        { workst = obj.WorkingStatus; }
                        var empdetails = db.TbEmployeeMasters.Where(x => x.EmployeeID == empid.Trim()).SingleOrDefault();
                        // Create Transaction
                        // Insert new Line
                        if (!string.IsNullOrEmpty(obj.ClockIn))
                        {
                            db.TbEmptransactions.Add(new TbEmptransaction()
                            {
                                TransactionNo = db.TbEmptransactions.Count() + 1,
                                TransactionDate = obj.TransactionDate,
                                EmployeeID = empid,
                                Shift = empdetails.ShiftID,
                                Line = empdetails.LineID,//obj.LineName,
                                WorkingStatus = workst,
                                ClockIn = "",
                                ClockOut = obj.ClockOut,
                                CreateDate = DateTime.Now,
                                CreateBy = User.Identity.Name,
                                UpdateDate = DateTime.Now,
                                UpdateBy = User.Identity.Name,
                            }); ;


                        }
                        else
                        {

                            db.TbEmptransactions.Add(new TbEmptransaction()
                            {
                                TransactionNo = db.TbEmptransactions.Count() + 1,
                                TransactionDate = obj.TransactionDate,
                                EmployeeID = empid,
                                Shift = empdetails.ShiftID,
                                Line = empdetails.LineID,//obj.LineName,
                                WorkingStatus = workst,
                                ClockOut = obj.ClockOut,
                                CreateDate = DateTime.Now,
                                CreateBy = User.Identity.Name,
                                UpdateDate = DateTime.Now,
                                UpdateBy = User.Identity.Name,
                            }); ;



                        }

                        
                        db.SaveChanges();

                    }
                }
                return RedirectToAction("EmployeeClockOut");

            }

        }



        //3.  Function Employee Clock in Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult EmployeeClockOutEdit(int id)
        {

            var Emps = db.View_Employee.Where(x => x.ID.Equals(id)).SingleOrDefault();
            return Json(Emps, JsonRequestBehavior.AllowGet);
        }


        //4.  Function Employee Clock In Update Transaction
        [HttpPost]
        public ActionResult EmployeeClockOutUpdate(TbEmptransaction obj)
        {

            // 1. check tbEmpTransaction == Null ?
            var Empdb = new TbEmptransaction();

            var EmpTran = db.TbEmptransactions.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.TransactionDate == DateTime.Today.Date).ToList();
            if (EmpTran.Count() != 0)
            {
                //Update Transaction
                Empdb = db.TbEmptransactions.Where(x => x.EmployeeID == obj.EmployeeID && x.TransactionDate == DateTime.Today.Date).SingleOrDefault();
                Empdb.ClockOut = obj.ClockOut;
                Empdb.UpdateBy = User.Identity.Name;
                Empdb.UpdateDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("EmployeeClockOut");
            }
            else
            {
                string workst;

                if (obj.WorkingStatus == null)
                { workst = "Working"; }
                else
                { workst = obj.WorkingStatus; }
                var empdetails = db.TbEmployeeMasters.Where(x => x.EmployeeID == obj.EmployeeID.Trim()).SingleOrDefault();
                // Create Transaction
                // Insert new Line               
                db.TbEmptransactions.Add(new TbEmptransaction()
                {
                    TransactionNo = db.TbEmptransactions.Count() + 1,
                    TransactionDate = DateTime.Today.Date,
                    EmployeeID = obj.EmployeeID,
                    Shift = 1,
                    Line = obj.Line,
                    Section = obj.Section,
                    WorkingStatus = workst,
                    ClockOut = obj.ClockOut,
                    CreateDate = DateTime.Now,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Now,
                    UpdateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }

            return RedirectToAction("EmployeeClockOut");

        }


        //End Employee Clock out
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Services Clock in Function
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ServicesClockIn(View_Employee obj)
        {

            if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
            {


                var tables = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_Employee = db.View_Employee.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),
                    view_ServicesClocktime = db.View_ServicesClocktime.ToList()

                };

                var ViewEmp = tables.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName)).ToList();
                tables.view_Employee = ViewEmp;

                return View(tables);

            }
            else
            {

                var tables = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_Employee = db.View_Employee.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),
                    view_ServicesClocktime = db.View_ServicesClocktime.ToList()

                };

                return View(tables);
            }



        }


        public ActionResult ServicesClockOut(View_Employee obj)
        {

            if (!string.IsNullOrEmpty(obj.EmployeeID) || !string.IsNullOrEmpty(obj.LineName))
            {


                var tables = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_Employee = db.View_Employee.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                var ViewEmp = tables.view_Employee.Where(x => x.EmployeeID.Equals(obj.EmployeeID) && x.LineName.Equals(obj.LineName)).ToList();
                tables.view_Employee = ViewEmp;

                return View(tables);

            }
            else
            {

                var tables = new ViewModelAll
                {
                    tbEmployeeMaster = db.TbEmployeeMasters.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbSection = db.TbSections.ToList(),
                    tbService = db.TbServices.ToList(),
                    view_Employee = db.View_Employee.ToList(),
                    tbShift = db.TbShifts.ToList(),
                    tbEmptransaction = db.TbEmptransactions.ToList(),

                };

                return View(tables);
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////
    }
}