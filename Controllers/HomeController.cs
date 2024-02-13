using PLIMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;
using System.Drawing;
using System.Web.WebPages.Scope;
using PLIMS.ViewModel;

namespace PLIMS.Controllers
{
    public class HomeController : Controller
    {
        PhlimsDatabaseEntities _db = new PhlimsDatabaseEntities();
      
        // GET: Home

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Employee");
            return View();
        }
        [HttpPost]
        public ActionResult Login(TbUser usersign)
        {
            var login = _db.TbUsers.FirstOrDefault(x => x.UserEmpID.Equals(usersign.UserEmpID)
                                                     && x.UserPassword.Equals(usersign.UserPassword));

            if (login != null)
            {
                FormsAuthentication.SetAuthCookie(login.ID.ToString(),true);
                return RedirectToAction("UserInformation","Employee");

            }
            else

            {
                ViewBag.Message = "ไม่พบชื่อผู้ใช้งาน";
            }
            return View();

        }


        public ActionResult UserInformation(TbUser users)
        {
            var tables = new ViewModelAll
            {
                tbUser = _db.TbUsers.ToList()
                

            };

            var user = tables.tbUser.Where(x => x.UserEmpID.Equals(users.UserEmpID)).ToList();

            return View();
        }
        public ActionResult Logout()
        {
            if(User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();
            return Redirect("Login");
        }

        public ActionResult UserRegisterList()
        {
            return View();
        }

        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegister(TbUser uservalue)
        {
            try
            {
                if (ModelState.IsValid && !checkusername(uservalue.UserEmpID))
                {

                    uservalue.Status = 1;
                    uservalue.CreateDate = DateTime.Now;
                    _db.TbUsers.Add(uservalue);
                    _db.SaveChanges();
                    ViewBag.Message = "ลงทะเบียนสำเร็จ";
                }
                else
                { 
                    ViewBag.Message = "ลงทะเบียนไม่สำเร็จ! กรุณาตรวจสอบข้อมูล"; 
                }

            }
            catch(Exception ex) {
                ViewBag.Message = ex.Message;
            }
            return View();
        }
        private bool checkusername(string uservalue)
        { 
            if(_db.TbUsers.Any(x => x.UserName.ToString() == uservalue) )
            {
                return false;
            }
            else
            {
                return true; 
            
            }
        }




        /// <summary>
        /// User Management
        /// </summary>
        /// <returns></returns>
        public ActionResult UserManagement()
        {


            var UserRecord = new ViewModelAll
            {
                tbRole = _db.TbRoles.ToList(),
                tbUser = _db.TbUsers.ToList(),
                view_PermissionMaster = _db.View_PermissionMaster.ToList(),
                view_User = _db.View_User.ToList(),
                tbPlants = _db.TbPlants.ToList(),
                tbLine = _db.TbLines.ToList(),
                tbSection = _db.TbSections.ToList(),
                tbEmployeeMaster = _db.TbEmployeeMasters.ToList()
            };


            return View(UserRecord);

        }


        // 1.  Function  Edit Transaction : ฟังก์ชั่นนี้ใช่ร่วมกับ Update function
        [HttpGet]
        public JsonResult UserEdit(int id)
        {
            var users = _db.View_User.Find(id);
            return Json(users, JsonRequestBehavior.AllowGet);
        }


        // 3. Function  Inactive transaction
        public JsonResult UserInactive(int id)
        {
            var Empdb = _db.TbUsers.Where(p => p.ID.Equals(id)).SingleOrDefault();
            if (Empdb != null)
            {
                Empdb.Status = 0;
                Empdb.UpdateBy = User.Identity.Name;
                Empdb.UpdateDate = DateTime.Now;
                _db.SaveChanges();

            }

            return Json(_db.TbUsers, JsonRequestBehavior.AllowGet);

        }


        // 4. Function Plant Create transaction
        [HttpPost]
        public ActionResult UserCreate(View_User obj)
        {
            //Check Duplicate
            
            var userdb = _db.TbUsers.Where(x => x.UserEmpID.Equals(obj.UserEmpID));
            var roledb = _db.TbRoles.Where(x => x.RoleName == obj.RoleName.Trim()).SingleOrDefault();
                    if (userdb.Count() == 0)
            {
                // Insert new Plant               
                _db.TbUsers.Add(new TbUser()
                {
                    ID = _db.TbUsers.Count() + 1,
                    UserName = obj.UserName,
                    UserLastName = obj.UserLastName,
                    UserPassword = obj.UserPassword,
                    UserEmpID = obj.UserEmpID,
                    UserEmail = obj.UserEmail,
                    UserPermission = roledb.RoleID,
                    Status = 1,
                    CreateDate = DateTime.Today,
                    CreateBy = User.Identity.Name
                });
                _db.SaveChanges();

            }
            else
            {
                ViewBag.Error = "User is Duplicate!";
            }
            return RedirectToAction("UserManagement");
        }


        // User Add New
        public ActionResult UserManagementsave(TbUser user)
            {
                    bool status = false;

                    if (ModelState.IsValid)
                    {
                        using (PhlimsDatabaseEntities db = new PhlimsDatabaseEntities()) 
                        { 
                
                            db.TbUsers.Add(user);
                            db.SaveChanges();
                            status = true;
                
                        }
             
                     }
               
                    return new JsonResult { Data = new { status = status } };     
            }


        public ActionResult RoleManagement()
        {


             var role = new ViewModelAll
            {
                tbPage = _db.TbPages.ToList(),
                tbRole = _db.TbRoles.ToList(),
                tbPermission = _db.TbPermissions.ToList(),
                view_PagePermission = _db.View_PagePermission.ToList()

            };

 

            //var RoleTb = from s in role.view_PagePermission
            //            group s by s.RoleName  into g
            //            select new 
            //            {
            //                //rowKey = g.PermissionID,
            //                RoleAction =  g.Select(s => new { RoleAction = s.RoleAction }).ToList()
            //            };



            return View(role);



        }

       public ActionResult SetUpRefreshtime()
    {

        return View();
    }


        public ActionResult RollBackDataProduction()
        {

            return View();
        }


        public ActionResult ManualImportData()
        {

            return View();
        }




















    }



}