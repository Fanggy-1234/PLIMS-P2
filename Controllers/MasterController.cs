using PLIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Data.OleDb;
using System.Data;
using LinqToExcel;
using System.Data.Entity.Validation;
using System.Web.UI.WebControls;
using DataSet = System.Data.DataSet;
using System.Runtime.Remoting.Contexts;
using static System.Collections.Specialized.BitVector32;
using PLIMS.ViewModel;


namespace PLIMS.Controllers
{
    public class MasterController : Controller
    {
        PhlimsDatabaseEntities db = new PhlimsDatabaseEntities();

 
        int lastdata = 0;
        // GET: Master

        public ActionResult Index()
        {
            lastdata = db.TbPlants.Count();
            return View();
        }

        /// <summary>
        /// Master : Plant
        /// </summary>
        /// <returns></returns>
        public ActionResult Plant()
        {
            ModelState.Clear();
            TbPlant plant = new TbPlant();
            return View(plant);

        }


        // 2. Function Plant show filler information
        [HttpGet]
        public ActionResult Plant(TbPlant obj)
        {

            if (!string.IsNullOrEmpty(obj.PlantName) || obj.PlantID != 0)
            {
                var plantdb = from p in db.TbPlants
                              select p;
                plantdb = plantdb.Where(p => p.PlantName.Equals(obj.PlantName) || p.PlantID.Equals(obj.PlantID));
                return View(plantdb.ToList());
            }
            else
            {
                return View(db.TbPlants.ToList());

            }

        }


        // 3. Function Plant Create transaction
        [HttpPost]
        public ActionResult CreatePlant(TbPlant obj)
        {
            //Check Duplicate
            var plantdb = db.TbPlants.Where(p => p.PlantName.Equals(obj.PlantName));
            var userdb = db.TbUsers.Where(x => x.ID.Equals(User.Identity.Name)).SingleOrDefault();
            if (plantdb.Count() == 0)
            {
                // Insert new Plant               
                db.TbPlants.Add(new TbPlant()
                {
                    PlantID = db.TbPlants.Count() + 1,
                    PlantName = obj.PlantName,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = userdb.UserEmpID
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Plant Duplicate!";
            }
            return RedirectToAction("Plant");
        }


        // 4. Function Plant Clear Fillter
        public ActionResult PlantClear()
        {
            return View(db.TbPlants.ToList());

        }

        // 5.  Function Plant Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult PlantEdit(int id)
        {
            var plant = db.TbPlants.Find(id);
            return Json(plant, JsonRequestBehavior.AllowGet);
        }

        // 6. Function Plant Update transaction
        [HttpPost]
        public ActionResult PlantUpdate(TbPlant obj)
        {

            var plantdb = db.TbPlants.Where(x => x.PlantID == obj.PlantID).SingleOrDefault();
            plantdb.PlantName = obj.PlantName;
            if (obj.Status == 1) { plantdb.Status = 1; } else { plantdb.Status = 0; };
            plantdb.UpdateBy = User.Identity.Name;
            plantdb.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Plant");

        }

        // 7. Function Plant Inactive transaction
        public JsonResult PlantInactive(int id)
        {
            var Plantdb = db.TbPlants.Where(p => p.PlantID.Equals(id)).SingleOrDefault();
            if (Plantdb != null)
            {
                Plantdb.Status = 0;
                Plantdb.UpdateBy = User.Identity.Name;
                Plantdb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbPlants, JsonRequestBehavior.AllowGet);

        }
        //End Master Plant
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////




        /// <summary>
        /// Master 2. Line
        /// </summary>
        /// <returns></returns>
        public ActionResult Line()
        {
            lastdata = db.TbLines.Count();
            return View(db.TbLines.ToList());

        }


        //2. Function Line show filler information
        [HttpGet]
        public ActionResult Line(TbLine obj)
        {

            if (!string.IsNullOrEmpty(obj.LineName) || obj.LineID != 0)
            {
                var Linedb = from p in db.TbLines
                             select p;
                Linedb = Linedb.Where(p => p.LineName.Equals(obj.LineName) || p.LineID.Equals(obj.LineID));
                return View(Linedb.ToList());
            }
            else
            {
                return View(db.TbLines.ToList());

            }
        }

        // 3.Function Line Clear fillter

        [HttpPost]
        public ActionResult CreateLine(TbLine obj)
        {

            // Insert new Line               
            db.TbLines.Add(new TbLine()
            {
                LineID = db.TbLines.Count() + 1,
                LineName = obj.LineName,
                Status = 1,
                CreateDate = DateTime.Today,
                CreateBy = User.Identity.Name,
                UpdateDate = DateTime.Today,
                UpdateBy = User.Identity.Name,
            });
            db.SaveChanges();


            return RedirectToAction("Line");
        }

        //4. Function Line Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult LineEdit(int id)
        {
            var Line = db.TbLines.Find(id);
            return Json(Line, JsonRequestBehavior.AllowGet);
        }

        // 5. Function Line Update Transaction
        [HttpPost]
        public ActionResult LineUpdate(TbLine obj)
        {
            var Linedb = db.TbLines.Where(x => x.LineID == obj.LineID).SingleOrDefault();
            if (obj.LineName != null)
            {
                Linedb.LineName = obj.LineName;

            }
            if (obj.Status != null)
            {
                Linedb.Status = obj.Status;
            }

            Linedb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Line");
        }


        // 6. Function Line Create transaction
        [HttpPost]
        public ActionResult LineCreate(TbLine obj)
        {
            //Check Duplicate
            var Linedb = db.TbLines.Where(p => p.LineName.Equals(obj.LineName));
            if (Linedb.Count() == 0)
            {
                // Insert new Line
                db.TbLines.Add(new TbLine()
                {
                    LineID = db.TbLines.Count() + 1,
                    LineName = obj.LineName,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Today,
                    UpdateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Line Duplicate!";
            }
            return RedirectToAction("Line");
        }


        // 7. Function Line Inactive transaction
        public JsonResult LineInactive(int id)
        {
            var Linedb = db.TbLines.Where(p => p.LineID.Equals(id)).SingleOrDefault();
            if (Linedb != null)
            {
                Linedb.Status = 0;
                Linedb.UpdateBy = User.Identity.Name;
                Linedb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbLines, JsonRequestBehavior.AllowGet);

        }

 

        /// 8. Function Services Download Excel
        public void LineDownloadExcel()
        {
            var services = db.TbLines;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Line");
            Sheet.Cells["A1"].Value = "LineID";
            Sheet.Cells["B1"].Value = "LineName";
            Sheet.Cells["C1"].Value = "Status";
            int row = 2;
            foreach (var item in services)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.LineID;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.LineName;
                Sheet.Cells[string.Format("C{0}", row)].Value = "1";
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "LineReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }



        // 9. Function Line Import Excel
        public ActionResult LineUpload(FormCollection formCollection)
        {
            var LinelistUpdate = new List<TbLine>();
            var LinelistCreate = new List<TbLine>();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    int cntlastcreate = 0;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                // update
                                var ExcelSerID = Int32.Parse((workSheet.Cells[rowIterator, 1].Value).ToString());
                                var LineUpdate = db.TbLines.Where(x => x.LineID == ExcelSerID).SingleOrDefault();
                                LineUpdate.LineName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                LineUpdate.Status = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                LineUpdate.UpdateDate = DateTime.Now;
                                LineUpdate.UpdateBy = User.Identity.Name;
                                db.SaveChanges();
                            }
                            else
                            {
                                //create
                                //Check Duplicated
                                var LineName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();

                                var LineUpdate = db.TbLines.Where(x => x.LineName == LineName).SingleOrDefault();
                                if (LineUpdate == null)
                                {
                                    var LineCreate = new TbLine();
                                    var cntlineCreate = db.TbLines.Count();
                                    LineCreate.LineID = cntlastcreate + cntlineCreate + 1;
                                    LineCreate.LineName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                    LineCreate.Status = '1';
                                    LineCreate.CreateDate = DateTime.Now;
                                    LineCreate.CreateBy = User.Identity.Name;
                                    db.TbLines.Add(LineCreate);
                                    db.SaveChanges();
                                    cntlastcreate = +1;
                                }
                                else
                                {
                                    ViewBag.Error = "Line :" + rowIterator + "is Duplicate";

                                }

                            }


                        }
                    }
                }
            }

            return RedirectToAction("Line");


        }

        // End Master Line
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Master 3. Product
        /// </summary>
        /// <returns></returns>

        public ActionResult Product()
        {
            lastdata = db.TbLines.Count();
            return View(db.TbLines.ToList());

        }


        //2. Function Product show filler information
        [HttpGet]
        public ActionResult Product(TbProduct obj)
        {

            if (!string.IsNullOrEmpty(obj.ProductName) || obj.ProductID != 0)
            {
                var productdb = from p in db.TbProducts
                                select p;
                productdb = productdb.Where(p => p.ProductName.Equals(obj.ProductName) || p.ProductID.Equals(obj.ProductID));
                return View(productdb.ToList());
            }
            else
            {
                return View(db.TbProducts.ToList());

            }



        }


        // 3.Function Product Clear fillter
        public ActionResult ProductClear()
        {          
            return View(db.TbProducts.ToList());

        }



        //4. Function product Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult ProductEdit(int id)
        {
            var product = db.TbProducts.Find(id);
            return Json(product, JsonRequestBehavior.AllowGet);

        }


        // 5. Function product Update Transaction
        [HttpPost]
        public ActionResult ProductUpdate(TbProduct obj)
        {

            var productdb = db.TbProducts.Where(x => x.ProductID == obj.ProductID).SingleOrDefault();
            if (obj.ProductName != null)
            {
                productdb.ProductName = obj.ProductName;

            }
            productdb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Product");

        }


        // 6. Function Product Create transaction
        [HttpPost]
        public ActionResult ProductCreate(TbProduct obj)
        {
            //Check Duplicate
            var Productdb = db.TbProducts.Where(p => p.ProductName.Equals(obj.ProductName));
            if (Productdb.Count() == 0)
            {
                // Insert new Product               
                db.TbProducts.Add(new TbProduct()
                {
                    ProductID = db.TbProducts.Count() + 1,
                    ProductName = obj.ProductName,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Today,
                    UpdateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Product Duplicate!";
            }
            return RedirectToAction("Product");
        }


        // 7. Function Product Inactive transaction
        public JsonResult ProductInactive(int id)
        {
            var Producdb = db.TbProducts.Where(p => p.ProductID.Equals(id)).SingleOrDefault();
            if (Producdb != null)
            {
                Producdb.Status = 0;
                Producdb.UpdateBy = User.Identity.Name;
                Producdb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbProducts, JsonRequestBehavior.AllowGet);

        }



        /// 8. Function Product Download Excel
        public void ProductDownloadExcel()
        {
            var Product = db.TbProducts;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Product");
            Sheet.Cells["A1"].Value = "ProductID";
            Sheet.Cells["B1"].Value = "ProductName";
            Sheet.Cells["C1"].Value = "Status";
            int row = 2;
            foreach (var item in Product)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.ProductID;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.ProductName;
                Sheet.Cells[string.Format("C{0}", row)].Value = "1";
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ProductReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }



        // 9. Function Product Import Excel
        public ActionResult ProductUpload(FormCollection formCollection)
        {
            var ProductlistUpdate = new List<TbProduct>();
            var ProductlistCreate = new List<TbProduct>();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    int cntlastcreate = 0;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                // update
                                var ExcelProID = Int32.Parse((workSheet.Cells[rowIterator, 1].Value).ToString());
                                var ProUpdate = db.TbProducts.Where(x => x.ProductID == ExcelProID).SingleOrDefault();
                                ProUpdate.ProductName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                ProUpdate.Status = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                ProUpdate.UpdateDate = DateTime.Now;
                                ProUpdate.UpdateBy = User.Identity.Name;
                                db.SaveChanges();
                            }
                            else
                            {
                                //create
                                //Check Duplicate
                                var proName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                var proUpdate = db.TbProducts.Where(
                                                x => x.ProductName == proName 
                                                ).SingleOrDefault();
                                if (proUpdate == null)
                                {
                                    var proCreate = new TbProduct();
                                    var cntserCreate = db.TbServices.Count();
                                    proCreate.ProductID = cntlastcreate + cntserCreate + 1;
                                    proCreate.ProductName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                    proCreate.Status = '1';
                                    proCreate.CreateDate = DateTime.Now;
                                    proCreate.CreateBy = User.Identity.Name;
                                    db.TbProducts.Add(proCreate);
                                    db.SaveChanges();
                                    cntlastcreate = +1;
                                }
                                else
                                {
                                    ViewBag.Error = "Product :" + rowIterator + "is Duplicate";

                                }

                            }


                        }
                    }
                }
            }

            return RedirectToAction("Product");

        }

        // End Master Product
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Master Section
        /// </summary>
        /// <returns></returns>

        public ActionResult Section()
        {
            lastdata = db.TbSections.Count();
            return View(db.TbSections.ToList());

        }

        //2. Function Section show filler information
        [HttpGet]
        public ActionResult Section(TbSection obj)
        {

            if (!string.IsNullOrEmpty(obj.SectionName) || obj.SectionID != 0)
            {
                var sectiondb = from p in db.TbSections
                                select p;
                sectiondb = sectiondb.Where(p => p.SectionName.Equals(obj.SectionName) || p.SectionID.Equals(obj.SectionID));
                return View(sectiondb.ToList());
            }
            else
            {
                return View(db.TbSections.ToList());

            }



        }

        // 3.Function Section Clear fillter
        public ActionResult SectionClear()
        {

            return View(db.TbSections.ToList());

        }



        //4. Function Section Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult SectionEdit(int id)
        {
            var Section = db.TbSections.Find(id);
            return Json(Section, JsonRequestBehavior.AllowGet);

        }


        // 5. Function Section Update Transaction
        [HttpPost]
        public ActionResult SectionUpdate(TbSection obj)
        {

            var Sectiondb = db.TbSections.Where(x => x.SectionID == obj.SectionID).SingleOrDefault();
            if (obj.SectionName != null)
            {
                Sectiondb.SectionName = obj.SectionName;

            }
            if (obj.Delaytime != null)
            {
                Sectiondb.Delaytime = obj.Delaytime;

            }
            if (obj.Status != null)
            {
                Sectiondb.Status = obj.Status;
            }

            Sectiondb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Section");

        }


        // 6. Function Section Create transaction
        [HttpPost]
        public ActionResult SectionCreate(TbSection obj)
        {
            //Check Duplicate
            var Sectiondb = db.TbSections.Where(p => p.SectionName.Equals(obj.SectionName));
            if (Sectiondb.Count() == 0)
            {
                // Insert new Plant

                db.TbSections.Add(new TbSection()
                {
                    SectionID = db.TbSections.Count() + 1,
                    SectionName = obj.SectionName,
                    Delaytime = obj.Delaytime,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Section Duplicate!";
            }
            return RedirectToAction("Section");
        }

        // 7. Function Section Inactive transaction
        public JsonResult SectionInactive(int id)
        {
            var Sectiondb = db.TbSections.Where(p => p.SectionID.Equals(id)).SingleOrDefault();
            if (Sectiondb != null)
            {
                Sectiondb.Status = 0;
                Sectiondb.UpdateBy = User.Identity.Name;
                Sectiondb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbSections, JsonRequestBehavior.AllowGet);

        }


        /// 8. Function Section Download Excel
        public void SectionDownloadExcel()
        {
            var Section = db.TbSections;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Section");
            Sheet.Cells["A1"].Value = "SectionID";
            Sheet.Cells["B1"].Value = "SectionName";
            Sheet.Cells["C1"].Value = "DelayTime";
            Sheet.Cells["D1"].Value = "Status";
            int row = 2;
            foreach (var item in Section)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.SectionID;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.SectionName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Delaytime;
                Sheet.Cells[string.Format("D{0}", row)].Value = "1";
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "SectionReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }



        // 9. Function Section Import Excel
        public ActionResult SectionUpload(FormCollection formCollection)
        {
            var SectionlistUpdate = new List<TbSection>();
            var SectionlistCreate = new List<TbSection>();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    int cntlastcreate = 0;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                // update
                                var ExcelSecID = Int32.Parse((workSheet.Cells[rowIterator, 1].Value).ToString());
                                var SecUpdate = db.TbSections.Where(x => x.SectionID == ExcelSecID).SingleOrDefault();
                                SecUpdate.SectionName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                SecUpdate.Delaytime = workSheet.Cells[rowIterator, 3].Value.ToString().Trim();
                                SecUpdate.Status = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                SecUpdate.UpdateDate = DateTime.Now;
                                SecUpdate.UpdateBy = User.Identity.Name;
                                db.SaveChanges();
                            }
                            else
                            {
                                //create
                                //Check Duplicate
                                var SecName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                var SecUpdate = db.TbSections.Where(
                                                x => x.SectionName == SecName 
                                                ).SingleOrDefault();
                                if (SecUpdate == null)
                                {
                                    var SecCreate = new TbSection();
                                    var cntsecCreate = db.TbSections.Count();
                                    SecCreate.SectionID = cntlastcreate + cntsecCreate + 1;
                                    SecCreate.SectionName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                    SecCreate.Delaytime = workSheet.Cells[rowIterator, 3].Value.ToString().Trim();
                                    SecCreate.Status = '1';
                                    SecCreate.CreateDate = DateTime.Now;
                                    SecCreate.CreateBy = User.Identity.Name;
                                    db.TbSections.Add(SecCreate);
                                    db.SaveChanges();
                                    cntlastcreate = +1;
                                }
                                else
                                {
                                    ViewBag.Error = "Section :" + rowIterator + "is Duplicate";

                                }

                            }


                        }
                    }
                }
            }

            return RedirectToAction("Section");


        }
        // End Master Section
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Master  Shift
        /// </summary>
        /// <returns></returns>


        public ActionResult Shift()
        {
            lastdata = db.TbShifts.Count();
            return View(db.TbShifts.ToList());

        }



        //2. Function Shift show filler information
        [HttpGet]
        public ActionResult Shift(TbShift obj)
        {

            if (!string.IsNullOrEmpty(obj.ShiftName) || obj.ShiftID != 0)
            {
                var shiftdb = from p in db.TbShifts
                              select p;
                shiftdb = shiftdb.Where(p => p.ShiftName.Equals(obj.ShiftName) || p.ShiftID.Equals(obj.ShiftID));
                return View(shiftdb.ToList());
            }
            else
            {
                return View(db.TbShifts.ToList());

            }



        }


        // 3.Function Shift Clear fillter

        public ActionResult ShiftClear()
        { 
            return View(db.TbShifts.ToString());

        }



        //4. Function Shift Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult ShiftEdit(int id)
        {
            var Shift = db.TbShifts.Find(id);
            return Json(Shift, JsonRequestBehavior.AllowGet);

        }


        // 5. Function Shift Update Transaction
        [HttpPost]
        public ActionResult ShiftUpdate(TbShift obj)
        {

            var Shiftdb = db.TbShifts.Where(x => x.ShiftID == obj.ShiftID).SingleOrDefault();
            if (obj.ShiftName != null)
            {
                Shiftdb.ShiftName = obj.ShiftName;

            }
            if (obj.StartTime != null)
            {
                Shiftdb.StartTime = obj.StartTime;
            }
            if (obj.EndTime != null)
            {
                Shiftdb.EndTime = obj.EndTime;
            }
            if (obj.Status != null)
            {
                Shiftdb.Status = obj.Status;
            }

            Shiftdb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Shift");

        }


        // 6. Function Shift Create transaction
        [HttpPost]
        public ActionResult ShiftCreate(TbShift obj)
        {
            //Check duplicate
            var Shiftdb = db.TbShifts.Where(p => p.ShiftName.Equals(obj.ShiftName));
            if (Shiftdb.Count() == 0)
            {
                // Insert new Shift               
                db.TbShifts.Add(new TbShift()
            {
                ShiftID = db.TbShifts.Count() + 1,
                ShiftName = obj.ShiftName,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                Status = 1,
                CreateDate = DateTime.Today,
                CreateBy = User.Identity.Name,
                UpdateDate = DateTime.Today,
                UpdateBy = User.Identity.Name,
            });
            db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Shift Duplicate!";
            }

            return RedirectToAction("Shift");
        }


        // 7. Function Shift Inactive transaction
        public JsonResult ShiftInactive(int id)
        {
            var Shiftdb = db.TbShifts.Where(p => p.ShiftID.Equals(id)).SingleOrDefault();
            if (Shiftdb != null)
            {
                Shiftdb.Status = 0;
                Shiftdb.UpdateBy = User.Identity.Name;
                Shiftdb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbShifts, JsonRequestBehavior.AllowGet);

        }



        // End Master Shift
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Master  Incentive
        /// </summary>
        /// <returns></returns>
        public ActionResult Incentive()
        {



            var IncentiveRecord = new ViewModelAll
            {
                tbIncentiveMaster = db.TbIncentiveMasters.ToList(),
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                tbProduct = db.TbProducts.ToList(),
                tbSection = db.TbSections.ToList(),
                View_Incentives = db.View_Incentive.ToList(),

            };

            return View(IncentiveRecord);




        }


        //2. Function Services show filler information
        [HttpGet]
        public ActionResult Incentive(View_Incentive obj)
        {


            if (!string.IsNullOrEmpty(obj.IncentiveName) || !string.IsNullOrEmpty(obj.PlantName) || !string.IsNullOrEmpty(obj.LineName) || !string.IsNullOrEmpty(obj.ProductName))
            {

                var incentives = new ViewModelAll
                {
                    tbIncentiveMaster = db.TbIncentiveMasters.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbProduct = db.TbProducts.ToList(),
                    tbSection = db.TbSections.ToList(),
                    View_Incentives = db.View_Incentive.ToList(),


                };

                //if(!string.IsNullOrEmpty(obj.IncentiveName) && string.IsNullOrEmpty(obj.PlantName) && string.IsNullOrEmpty(obj.LineName) && string.IsNullOrEmpty(obj.ProductName))
                //{
                //   var  ViewEmpIncentiveName = incentives.View_Incentives.Where(x => x.IncentiveName.Equals(obj.IncentiveName)).ToList();
                //    incentives.View_Incentives = ViewEmpIncentiveName;
                //}


                //if (string.IsNullOrEmpty(obj.IncentiveName) && !string.IsNullOrEmpty(obj.PlantName) && string.IsNullOrEmpty(obj.LineName) && string.IsNullOrEmpty(obj.ProductName))
                //{
                //    var ViewEmpPlantName = incentives.View_Incentives.Where(x => x.PlantName.Equals(obj.PlantName)).ToList();
                //    incentives.View_Incentives = ViewEmpPlantName;
                //}


                //if (string.IsNullOrEmpty(obj.IncentiveName) && string.IsNullOrEmpty(obj.PlantName) && !string.IsNullOrEmpty(obj.LineName) && string.IsNullOrEmpty(obj.ProductName))
                //{
                //    var ViewEmpPlantName = incentives.View_Incentives.Where(x => x.LineName.Equals(obj.LineName)).ToList();
                //    incentives.View_Incentives = ViewEmpPlantName;
                //}


                //if (string.IsNullOrEmpty(obj.IncentiveName) && string.IsNullOrEmpty(obj.PlantName) && string.IsNullOrEmpty(obj.LineName) && !string.IsNullOrEmpty(obj.ProductName))
                //{
                //    var ViewEmpPlantName = incentives.View_Incentives.Where(x => x.ProductName.Equals(obj.ProductName)).ToList();
                //    incentives.View_Incentives = ViewEmpPlantName;
                //}


                var ViewEmpPlantName = incentives.View_Incentives.Where(x => x.IncentiveName.Equals(obj.IncentiveName) && x.PlantName.Equals(obj.PlantName) && x.LineName.Equals(obj.LineName) && x.ProductName.Equals(obj.ProductName)).ToList();
                incentives.View_Incentives = ViewEmpPlantName;

                return View(incentives);

            }
            else
            {

                var Employee = new ViewModelAll
                {
                    tbIncentiveMaster = db.TbIncentiveMasters.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbProduct = db.TbProducts.ToList(),
                    tbSection = db.TbSections.ToList(),
                    View_Incentives = db.View_Incentive.ToList(),

                };

                return View(Employee);
            }






        }



        // 3.Function Incentive Clear fillter
        public ActionResult IncentiveClear()
        {
            var Employee = new ViewModelAll
            {
                tbIncentiveMaster = db.TbIncentiveMasters.ToList(),
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                tbProduct = db.TbProducts.ToList(),
                tbSection = db.TbSections.ToList(),
                View_Incentives = db.View_Incentive.ToList(),

            };

            return View(Employee);
        

    }


        //4. Function Incentive Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult IncentiveEdit(int id)
        {
            var Incentive = db.View_Incentive.Find(id);
            return Json(Incentive, JsonRequestBehavior.AllowGet);

        }



        // 5. Function Incentive Update Transaction
        [HttpPost]
        public ActionResult IncentiveUpdate(TbIncentiveMaster obj)
        {

            var Incentivedb = db.TbIncentiveMasters.Where(x => x.IncentiveID == obj.IncentiveID).SingleOrDefault();
            if (obj.IncentiveName != null)
            {
                Incentivedb.IncentiveName = obj.IncentiveName;

            }
            if (obj.PlantID != null)
            {
                Incentivedb.PlantID = obj.PlantID;

            }
            if (obj.LineID != null)
            {
                Incentivedb.LineID = obj.LineID;
            }
            if (obj.ProductID != null)
            {
                Incentivedb.ProductID = obj.ProductID;
            }
            if (obj.SectionID != null)
            {
                Incentivedb.SectionID = obj.SectionID;
            }
            if(obj.Min != null)
            {
                Incentivedb.Min = obj.Min;
            }
            if (obj.Max != null)
            {
                Incentivedb.Max = obj.Max;
            }
            if (obj.Rate != null)
            {
                Incentivedb.Rate = obj.Rate;
            }
            if (obj.Grade != null)
            {
                Incentivedb.Grade = obj.Grade;
            }
            if (obj.MonthYear != null)
            {
                string[] MonthYearArr = obj.MonthYear.Split('-');
                Incentivedb.MonthYear = MonthYearArr[1] + "/" + MonthYearArr[0];
            }
            if (obj.Status != null)
            {
                Incentivedb.Status = obj.Status;
            }

            Incentivedb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Incentive");

        }



        // 6. Function Services Create transaction
        [HttpPost]
        public ActionResult IncentiveCreate(ViewModelAll obj , View_Incentive tbincentive)
        {
            //Check Duplicate
              var plantid = db.TbPlants.Where(x => x.PlantName.Equals(tbincentive.PlantName)).SingleOrDefault();
                var lineid = db.TbLines.Where(x => x.LineName.Equals(tbincentive.LineName)).SingleOrDefault();
                var productid = db.TbProducts.Where(x => x.ProductName.Equals(tbincentive.ProductName)).SingleOrDefault();
                var sectionid = db.TbSections.Where(x => x.SectionName.Equals(tbincentive.SectionName)).SingleOrDefault();

            var plantdb = db.TbIncentiveMasters.Where(p => p.PlantID == plantid.PlantID && p.LineID == lineid.LineID && p.ProductID == productid.ProductID && p.SectionID == sectionid.SectionID).ToList();
            if (plantdb.Count() == 0)
            {
            
                // string[] MonthYearArr = obj.MonthYear.Split('-');
                // Insert new Plant

                db.TbIncentiveMasters.Add(new TbIncentiveMaster()
                {
                    IncentiveID = db.TbIncentiveMasters.Count() + 1,
                    IncentiveName = tbincentive.IncentiveName,
                    PlantID = plantid.PlantID,
                    LineID = lineid.LineID,
                    ProductID = productid.ProductID,
                    SectionID = sectionid.SectionID,
                    Min = tbincentive.Min,
                    Max = tbincentive.Max,
                    Rate = tbincentive.Rate,
                    Grade = tbincentive.Grade,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name,
                }) ;
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Incentive Duplicate!";
                //  return PartialView("Plant",obj);
            }
            return RedirectToAction("Incentive");
        }



        // 7. Function Incentive Inactive transaction
        public JsonResult IncentiveInactive(int id)
        {
            var Incentivedb = db.TbIncentiveMasters.Where(p => p.IncentiveID.Equals(id)).SingleOrDefault();
            if (Incentivedb != null)
            {

                Incentivedb.Status = 0;
                Incentivedb.UpdateBy = User.Identity.Name;
                Incentivedb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbIncentiveMasters, JsonRequestBehavior.AllowGet);
        }


        /// 8. Function Incentive Download Excel
        public void IncentiveDownloadExcel()
        {
            var Incentive = db.TbIncentiveMasters;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Incentive");
            Sheet.Cells["A1"].Value = "IncentiveID";
            Sheet.Cells["B1"].Value = "IncentiveName";
            Sheet.Cells["C1"].Value = "PlantID";
            Sheet.Cells["D1"].Value = "LineID";
            Sheet.Cells["E1"].Value = "ProductID";
            Sheet.Cells["F1"].Value = "SectionID";
            Sheet.Cells["G1"].Value = "Min"; ;
            Sheet.Cells["H1"].Value = "Max";
            Sheet.Cells["I1"].Value = "Rate";
            Sheet.Cells["J1"].Value = "Grade";
            Sheet.Cells["K1"].Value = "MonthYear";
            Sheet.Cells["L1"].Value = "Status";
            int row = 2;
            foreach (var item in Incentive)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.IncentiveID;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.IncentiveName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.PlantID;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.LineID;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.ProductID;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.SectionID;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Min;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.Max;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.Rate;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.Grade;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.MonthYear;
                Sheet.Cells[string.Format("L{0}", row)].Value = "1";

                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "IncentiveReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }



        // 9. Function Incentive Import Excel
        public ActionResult IncentiveUpload(FormCollection formCollection)
        {
            var IncentivelistUpdate = new List<TbIncentiveMaster>();
            var IncentivelistCreate = new List<TbIncentiveMaster>();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    int cntlastcreate = 0;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                // update
                                var ExcelIncID = Int32.Parse((workSheet.Cells[rowIterator, 1].Value).ToString());
                                var IncUpdate = db.TbIncentiveMasters.Where(x => x.IncentiveID == ExcelIncID).SingleOrDefault();
                                IncUpdate.IncentiveName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                IncUpdate.PlantID = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                IncUpdate.LineID = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                IncUpdate.ProductID = Convert.ToInt32(workSheet.Cells[rowIterator,5].Value);
                                IncUpdate.SectionID = Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value);
                                IncUpdate.Min = Convert.ToDecimal(workSheet.Cells[rowIterator, 7].Value);
                                IncUpdate.Max = Convert.ToDecimal(workSheet.Cells[rowIterator, 8].Value);
                                IncUpdate.Rate = Convert.ToDecimal(workSheet.Cells[rowIterator, 9].Value);
                                IncUpdate.Grade = workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                IncUpdate.MonthYear = workSheet.Cells[rowIterator, 11].Value.ToString().Trim();
                                IncUpdate.Status = Convert.ToInt32(workSheet.Cells[rowIterator, 12].Value);
                                IncUpdate.UpdateDate = DateTime.Now;
                                IncUpdate.UpdateBy = User.Identity.Name;
                                db.SaveChanges();
                            }
                            else
                            {
                                //create
                                //Check Duplicate
                                var IncName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                var IncPlant = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                var IncLine = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                var IncProduct = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                var IncSection = Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value);
                                var IncMin = Convert.ToDecimal(workSheet.Cells[rowIterator, 7].Value);
                                var IncMax = Convert.ToDecimal(workSheet.Cells[rowIterator, 8].Value);
                                var IncRate = Convert.ToDecimal(workSheet.Cells[rowIterator, 9].Value);
                                var IncGrade = workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                var IncMY = workSheet.Cells[rowIterator, 11].Value.ToString().Trim();
                                var serUpdate = db.TbIncentiveMasters.Where(
                                                x => x.PlantID == IncPlant &&
                                                x.LineID == IncLine &&
                                                x.ProductID == IncProduct &&
                                                x.SectionID == IncSection &&
                                                x.Min == IncMin &&
                                                x.Max == IncMax &&
                                                x.Rate == IncRate &&
                                                x.Grade == IncGrade &&
                                                x.MonthYear == IncMY 
                                                ).SingleOrDefault();
                                if (serUpdate == null)
                                {
                                    var IncCreate = new TbIncentiveMaster();
                                    var cntIncCreate = db.TbIncentiveMasters.Count();
                                    IncCreate.IncentiveID = cntlastcreate + cntIncCreate + 1;
                                    IncCreate.IncentiveName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                    IncCreate.PlantID = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                    IncCreate.LineID = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                    IncCreate.ProductID = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                    IncCreate.SectionID = Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value);
                                    IncCreate.Min = Convert.ToDecimal(workSheet.Cells[rowIterator, 7].Value);
                                    IncCreate.Max = Convert.ToDecimal(workSheet.Cells[rowIterator, 8].Value);
                                    IncCreate.Rate = Convert.ToDecimal(workSheet.Cells[rowIterator, 9].Value);
                                    IncCreate.Grade = workSheet.Cells[rowIterator, 10].Value.ToString().Trim();
                                    IncCreate.MonthYear = workSheet.Cells[rowIterator, 11].Value.ToString().Trim();
                                    IncCreate.Status = '1';
                                    IncCreate.CreateDate = DateTime.Now;
                                    IncCreate.CreateBy = User.Identity.Name;
                                    db.TbIncentiveMasters.Add(IncCreate);
                                    db.SaveChanges();
                                    cntlastcreate = +1;
                                }
                                else
                                {
                                    ViewBag.Error = "Incentive :" + rowIterator + "is Duplicate";

                                }

                            }


                        }
                    }
                }
            }

            return RedirectToAction("Incentive");


        }



        //End Master Incentive
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////




        /// <summary>
        /// Master : Sevices
        /// </summary>
        /// <returns></returns>
        public ActionResult Services()
        {

           
            var ServicesRecord = new ViewModelAll
            {
                tbService = db.TbServices.ToList(),
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                view_Services = db.View_Services.ToList()


            };

            return View(ServicesRecord);

        }

        // Function Services show filler information
        [HttpGet]
        public ActionResult Services(View_Services obj)
        {

            if (!string.IsNullOrEmpty(obj.ServicesName) || !string.IsNullOrEmpty(obj.Ser_PlantName) || !string.IsNullOrEmpty(obj.Ser_LineName))
            {


                var ServicesRecord = new ViewModelAll
                {
                    tbService = db.TbServices.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    view_Services = db.View_Services.ToList()


                };
                var Viewservicess = ServicesRecord.view_Services.Where(x => x.ServicesID.Equals(obj.ServicesID) || x.ServicesName.Equals(obj.ServicesName)|| x.Ser_PlantName.Equals(obj.Ser_PlantName) || x.Ser_LineName.Equals(obj.Ser_LineName)).ToList();
                ServicesRecord.view_Services = Viewservicess;

                return View(ServicesRecord);


            }
            else
            {
                var ServicesRecord = new ViewModelAll
                {
                    tbService = db.TbServices.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    view_Services = db.View_Services.ToList()


                };
                return View(ServicesRecord);


            }

        }

        // Function Services Clear fillter
        public ActionResult ServicesClear()
        {
            var ServicesRecord = new ViewModelAll
            {
                tbService = db.TbServices.ToList(),
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                view_Services = db.View_Services.ToList()


            };
            return View(ServicesRecord);

        }


        // Function Services Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult ServicesEdit(int id)
        {
            var service = db.View_Services.Find(id);
            return Json(service, JsonRequestBehavior.AllowGet);
        }


        // Function Services Update Transaction
        [HttpPost]
        public ActionResult ServicesUpdate(TbService obj)
        {

            var Servicedb = db.TbServices.Where(x => x.ServicesID == obj.ServicesID).SingleOrDefault();
            if (obj.ServicesName != null)
            {
                Servicedb.ServicesName = obj.ServicesName;

            }
            if (obj.Ser_PlantID != null)
            {
                Servicedb.Ser_PlantID = obj.Ser_PlantID;
            }
            if (obj.Ser_LineID != null)
            {
                Servicedb.Ser_LineID = obj.Ser_LineID;
            }
            if (obj.ServicesRate != null)
            {
                Servicedb.ServicesRate = obj.ServicesRate;
            }
            if (obj.MonthYear != null)
            {
                string[] MonthYearArr = obj.MonthYear.Split('-');
                Servicedb.MonthYear = MonthYearArr[1] + "/" + MonthYearArr[0];
            }
            if (obj.ServicesStatus != null)
            {
                Servicedb.ServicesStatus = obj.ServicesStatus;
            }

            Servicedb.UpdateBy = User.Identity.Name;
            obj.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Services");

        }


        // Function Services Create transaction
        [HttpPost]
        public ActionResult ServicesCreate(TbService obj)
        {
            //Check Duplicate
            var servicedb = db.TbServices.Where(p => p.ServicesName.Equals(obj.ServicesName));   
            if (servicedb.Count() == 0)
            {
                // Insert new Plant
                string[] MonthYearArr = obj.MonthYear.Split('-');
                db.TbServices.Add(new TbService()
                {
                    ServicesID = db.TbServices.Count() + 1,
                    ServicesName = obj.ServicesName,
                    Ser_PlantID = obj.Ser_PlantID,
                    Ser_LineID = obj.Ser_LineID,
                    ServicesRate = obj.ServicesRate,
                    MonthYear = MonthYearArr[1] + "/" + MonthYearArr[0],
                    ServicesStatus = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name,
                    UpdateDate = DateTime.Today,
                    UpdateBy = User.Identity.Name,
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Services Duplicate!";
            }
            return RedirectToAction("Services");
        }

        // Function Services Inactive transaction
        public JsonResult ServicesInactive(int id)
        {
            var servicedb = db.TbServices.Where(p => p.ServicesID.Equals(id)).SingleOrDefault();
            if (servicedb != null)
            {
                servicedb.Ser_LineID = 0;
                servicedb.UpdateBy = User.Identity.Name;
                servicedb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.View_Services, JsonRequestBehavior.AllowGet);

        }


        ///Function Services Download Excel
        public void ServicesDownloadExcel()
        {
            var services = db.TbServices;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Services");
            Sheet.Cells["A1"].Value = "ServicesID";
            Sheet.Cells["B1"].Value = "ServicesName";
            Sheet.Cells["C1"].Value = "PlantID";
            Sheet.Cells["D1"].Value = "LineID";
            Sheet.Cells["E1"].Value = "MonthYear";
            Sheet.Cells["F1"].Value = "ServicesRate";
            Sheet.Cells["G1"].Value = "ServiceStatus";
            int row = 2;
            foreach (var item in services)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.ServicesID;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.ServicesName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Ser_PlantID;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Ser_LineID;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.MonthYear;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.ServicesRate;
                Sheet.Cells[string.Format("G{0}", row)].Value = "1";
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ServicesReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }



        // Function Services Import Excel
        public ActionResult ServicesUpload(FormCollection formCollection)
        {
            var serviceslistUpdate = new List<TbService>();
            var serviceslistCreate = new List<TbService>();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    int cntlastcreate = 0;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {   
                            
                            if(workSheet.Cells[rowIterator, 1].Value != null )
                            {
                                // update
                                var ExcelSerID = Int32.Parse((workSheet.Cells[rowIterator, 1].Value).ToString());
                                var serUpdate = db.TbServices.Where(x => x.ServicesID == ExcelSerID).SingleOrDefault();
                                serUpdate.ServicesName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                serUpdate.Ser_PlantID = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                serUpdate.Ser_LineID = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                serUpdate.MonthYear = workSheet.Cells[rowIterator, 5].Value.ToString().Trim();
                                serUpdate.ServicesRate = workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                serUpdate.ServicesStatus = Convert.ToInt32(workSheet.Cells[rowIterator, 7].Value.ToString());
                                serUpdate.UpdateDate = DateTime.Now;
                                serUpdate.UpdateBy = User.Identity.Name;
                                db.SaveChanges();
                            }
                            else
                            {
                                //create
                                //Check Duplicate
                                var serName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                var serPlant = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                var serLine = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                var serMY = workSheet.Cells[rowIterator, 5].Value.ToString().Trim();
                                var serRate = workSheet.Cells[rowIterator, 6].Value.ToString().Trim();

                                var serUpdate = db.TbServices.Where(
                                                x => x.ServicesName == serName &&
                                                x.Ser_PlantID == serPlant  &&
                                                x.Ser_LineID == serLine &&
                                                x.MonthYear == serMY  &&
                                                x.ServicesRate == serRate
                                                ).SingleOrDefault();
                                if(serUpdate == null)
                                {
                                    var serCreate = new TbService();
                                    var cntserCreate = db.TbServices.Count();
                                    serCreate.ServicesID = cntlastcreate + cntserCreate + 1; 
                                    serCreate.ServicesName = workSheet.Cells[rowIterator, 2].Value.ToString().Trim();
                                    serCreate.Ser_PlantID = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                                    serCreate.Ser_LineID = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                                    serCreate.MonthYear = workSheet.Cells[rowIterator, 5].Value.ToString().Trim();
                                    serCreate.ServicesRate = workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                    serCreate.ServicesStatus = '1';
                                    serCreate.CreateDate = DateTime.Now;
                                    serCreate.CreateBy = User.Identity.Name;
                                    db.TbServices.Add(serCreate);
                                    db.SaveChanges();
                                    cntlastcreate = +1;
                                }
                                else
                                {
                                    ViewBag.Error = "Line :" + rowIterator + "is Duplicate";
                  
                                }
  
                            }
                            
                         
                        }
                    }
                }
            }
           
            return RedirectToAction("Services");


        }

        // End Master Services
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult Reason()
        {

            var reason = new ViewModelAll
            {
                tbReason = db.TbReasons.ToList(),
                tbPlants = db.TbPlants.ToList(),
                tbLine = db.TbLines.ToList(),
                tbProduct = db.TbProducts.ToList(),
                tbSection = db.TbSections.ToList(),
                view_Reason = db.View_Reason.ToList(),

            };

            return View(reason);
        }



        // 2. Function Plant show filler information
        [HttpGet]
        public ActionResult Reason(View_Reason obj)
        {

            if (!string.IsNullOrEmpty(obj.PlantName) || !string.IsNullOrEmpty(obj.LineName) || !string.IsNullOrEmpty(obj.ProductName) || !string.IsNullOrEmpty(obj.SectionName))
            {
                var reasondb = from p in db.View_Reason
                              select p;
                reasondb = reasondb.Where(p => p.PlantName.Equals(obj.PlantName) || p.LineName.Equals(obj.LineName) || p.ProductName.Equals(obj.ProductName) || p.SectionName.Equals(obj.SectionName));
                return View(reasondb.ToList());
            }
            else
            {

                var reason = new ViewModelAll
                {
                    tbReason = db.TbReasons.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbProduct = db.TbProducts.ToList(),
                    tbSection = db.TbSections.ToList(),
                    view_Reason = db.View_Reason.ToList(),

                };

                return View(reason);

            }

        }


        // 3. Function Plant Create transaction
        [HttpPost]
        public ActionResult CreateReason(View_Reason obj)
        {
            //Check Duplicate
            var plantdb = db.TbReasons.Where(p => p.ReasonID.Equals(obj.ReasonID));
            var userdb = db.TbUsers.Where(x => x.ID.Equals(User.Identity.Name)).SingleOrDefault();
            if (plantdb.Count() == 0)
            {
                // Insert new Plant               
                db.TbReasons.Add(new TbReason()
                {
                    ReasonID = db.TbPlants.Count() + 1,
                    ReasonName = obj.ReasonName,
                    PlantID = obj.PlantID,
                    LineID = obj.LineID,
                    SectionID = obj.SectionID,
                    ProductID = obj.ProductID,
                    Status = 1,
                    CreateDate = DateTime.Now,
                    CreateBy = userdb.UserEmpID
                });
                db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "Reason Duplicate!";
            }
            return RedirectToAction("Plant");
        }


        // 4. Function Plant Clear Fillter
        public ActionResult ReasonClear()
        {
            return View(db.View_Reason.ToList());

        }

        // 5.  Function Plant Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult ReasonEdit(int id)
        {
            var Reason = db.View_Reason.Find(id);
            return Json(Reason, JsonRequestBehavior.AllowGet);
        }

        // 6. Function Plant Update transaction
        [HttpPost]
        public ActionResult ReasonUpdate(View_Reason obj)
        {

         

            var Reasondb = db.TbReasons.Where(x => x.ReasonID == obj.ReasonID).SingleOrDefault();

            if (obj.ReasonName != null)
            {
                Reasondb.ReasonName = obj.ReasonName ;
            }
            if (obj.PlantName != null)
            {

                Reasondb.PlantID = obj.PlantID;
            }
            if (obj.LineName != null)
            {
                Reasondb.LineID = obj.LineID;
            }
            if (obj.ProductName != null)
            {
                Reasondb.ProductID = obj.ProductID;
            }
            if (obj.SectionName != null)
            {
                Reasondb.SectionID = obj.SectionID;
            }

            if (obj.Status == 1) { Reasondb.Status = 1; } else { Reasondb.Status = 0; };
            Reasondb.UpdateBy = User.Identity.Name;
            Reasondb.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Reason");

        }

        // 7. Function Plant Inactive transaction
        public JsonResult ReasonInactive(int id)
        {
            var Reasondb = db.TbReasons.Where(p => p.ReasonID.Equals(id)).SingleOrDefault();
            if (Reasondb != null)
            {
                Reasondb.Status = 0;
                Reasondb.UpdateBy = User.Identity.Name;
                Reasondb.UpdateDate = DateTime.Now;
                db.SaveChanges();

            }

            return Json(db.TbReasons, JsonRequestBehavior.AllowGet);

        }





        public ActionResult ProductSTD(View_PLPS obj)
        {

            if (!string.IsNullOrEmpty(obj.PlantName) || !string.IsNullOrEmpty(obj.LineName) || !string.IsNullOrEmpty(obj.ProductName) || !string.IsNullOrEmpty(obj.SectionName))
            {

                var tables = new ViewModelAll
                {
                    tbPLPS = db.TbPLPS.ToList(),
                    tbPlants = db.TbPlants.ToList(),
                    tbLine = db.TbLines.ToList(),
                    tbProduct = db.TbProducts.ToList(),
                    tbSection = db.TbSections.ToList(),
                    view_PLPS = db.View_PLPS.ToList()

                };

                var reason = tables.view_PLPS.Where(x => x.PlantName.Equals(obj.PlantName) && x.LineName.Equals(obj.LineName) && x.ProductName.Equals(obj.ProductName) && x.SectionName.Equals(obj.SectionName)).ToList();
            

                return View(reason);
            }
            else
            {

                var reason = new ViewModelAll
                        {
                            tbPLPS = db.TbPLPS.ToList(),
                            tbPlants = db.TbPlants.ToList(),
                            tbLine = db.TbLines.ToList(),
                            tbProduct = db.TbProducts.ToList(),
                            tbSection = db.TbSections.ToList(),
                            view_PLPS = db.View_PLPS.ToList(),

                        };

                        return View(reason);



            }


        }
        // End of controller





    }

}