using PLIMS.Models;
using Remotion.Mixins.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace PLIMS
{
    public class Utilities
    {
        public static TbUser GetCurrentUser()
        {
            PhlimsDatabaseEntities _db = new PhlimsDatabaseEntities();
            TbUser user = new TbUser();
            try
            {
                var username = Convert.ToInt32(HttpContext.Current.User.Identity.Name);   
                var checkuser = _db.TbUsers.Find(username);
               

                //if (checkuser.PasswordLastUpdate.Value.AddMonths(3) >=  DateTime.Now) 
                //{
                    
                //}
                if (checkuser != null)
                {
                    user.UserName = checkuser.UserName;
                    user.UserLastName = checkuser.UserLastName;
                    user.UserEmail = checkuser.UserEmail;
                    user.UserEmpID = checkuser.UserEmpID;
                    user.PasswordLastUpdate = checkuser.PasswordLastUpdate;
                    string[] linecon = checkuser.Lineconcern.Split(',');
                    for(int i = 1;i < linecon.Length; i++) {
                        var lineuser = _db.TbLines.Where(x => x.LineID.Equals(linecon[i]));
                      // ค้างเขียนโปแกรม line concern ไว้
                    }
                   

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }
    }
}