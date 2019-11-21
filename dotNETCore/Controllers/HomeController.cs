using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNETCore.Models;
using System.DirectoryServices.AccountManagement;
using Newtonsoft.Json;

namespace dotNETCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        public ActionResult OrgChart()
        {
            List<User> ADUsers = GetallAdUsers();
            return View(ADUsers);
        }

        //public ActionResult OrgChartGetManager()
        //{
        //    var SamAccountNAme = TempData["SamAccountName"];
        //    List<User> ADUsers = GetallAdUsers();
        //    return View("OrgChart", ADUsers);
        //}

        public EmptyResult UpdateUser(User user)
        {
            var ctx = new PrincipalContext(ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");
            UserPrincipalEx userPrin = UserPrincipalEx.FindByIdentity(ctx, IdentityType.DistinguishedName, user.Id);

            if (user.DisplayName != null)
            {
                userPrin.DisplayName = user.DisplayName;
            }
            if (user.SamAccountName != null)
            {
                userPrin.SamAccountName = user.SamAccountName;
            }
            if (user.JobTitle != null)
            {
                userPrin.Title = user.JobTitle;
            }
            if (user.Phone != null)
            {
                userPrin.TelephoneNumber = user.Phone;
            }
            if (user.Company != null)
            {
                userPrin.Company = user.Company;
            }
            
            userPrin.Save();

            return new EmptyResult();
        }



        //if you want to get Groups of Specific OU you have to add OU Name in Context        
        public static List<User> GetallAdUsers()
        {
            List<User> AdUsers = new List<User>();

  

            var ctx = new PrincipalContext(ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");


           


            UserPrincipal userPrin = new UserPrincipal(ctx);
            userPrin.Name = "*";
            var searcher = new PrincipalSearcher();
            searcher.QueryFilter = userPrin;
            var results = searcher.FindAll();

            //var id = 0;

            foreach (Principal p in results)
            {
              
                
                UserPrincipalEx extp = UserPrincipalEx.FindByIdentity(ctx, IdentityType.DistinguishedName, p.DistinguishedName);





                var managerCN = extp.Manager;

               

                string picture = "";

                if (extp.ThumbnailPhoto != null)
                {
                    picture = Convert.ToBase64String(extp.ThumbnailPhoto);
                }
                

                var mgr = "";

                string[] list = managerCN.Split(',');

                if (managerCN.Length > 0)
                {
                    mgr = list[0].Substring(3);
                }

                

                //id++;

                AdUsers.Add(new User
                {

                    Id = extp.DistinguishedName,                    
                    Pid = extp.Manager,
                    DisplayName = extp.DisplayName,
                    SamAccountName = extp.SamAccountName,
                    Manager = mgr,
                    Image = "data:image/jpeg;base64," + picture,
                    JobTitle = extp.Title,
                    Company = extp.Company,
                    Phone = extp.TelephoneNumber,
                    Disabled = (extp.Enabled == false) ? "disabled" : "enabled"
                });
               
            }



            //foreach (User u in AdUsers)
            //{


            //    var pid = AdUsers.Find(r => r.SamAccountName == u.ManagerCN);
            //    if (pid != null)
            //    {
            //        u.Pid = pid.Id;
            //    }

            //}


            return AdUsers;
            
        }

        public ActionResult ResetPassword(string id)
        {
            //i get the user by its SamaccountName to change his password
            PrincipalContext context = new PrincipalContext
                                       (ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");
            UserPrincipal user = UserPrincipal.FindByIdentity
                                 (context, IdentityType.DistinguishedName, id);
            //Enable Account if it is disabled
            user.Enabled = true;
            //Reset User Password
            string newPassword = "P@ssw0rd";
            user.SetPassword(newPassword);
            //Force user to change password at next logon (optional)
            user.ExpirePasswordNow();
            user.Save();
          
            return RedirectToAction("OrgChart");
        }

        public ActionResult DisableAccount(string id)
        {
            //i get the user by its SamaccountName to change his password
            PrincipalContext context = new PrincipalContext
                                       (ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");
            UserPrincipal user = UserPrincipal.FindByIdentity
                                 (context, IdentityType.DistinguishedName, id);
            user.Enabled = false;

            user.Save();

            return RedirectToAction("OrgChart");
        }

        public ActionResult EnableAccount(string id)
        {
            //i get the user by its SamaccountName to change his password
            PrincipalContext context = new PrincipalContext
                                       (ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");
            UserPrincipal user = UserPrincipal.FindByIdentity
                                 (context, IdentityType.DistinguishedName, id);
            user.Enabled = true;

            user.Save();

            return RedirectToAction("OrgChart");
        }


        public JsonResult AddAccount(string pid, string name)
        {
            var ctx = new PrincipalContext(ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");
            var up = new UserPrincipal(ctx, name, "QWEqwe123sdaf$g_sg", true);
            up.Save();


            //var ctx = new PrincipalContext(ContextType.Domain, "ad.balkangraph.com", "OU=TestOU,DC=ad,DC=balkangraph,DC=com");





            UserPrincipal userPrin = new UserPrincipal(ctx);
            userPrin.Name = "*";
            var searcher = new PrincipalSearcher();
            searcher.QueryFilter = userPrin;
            var results = searcher.FindAll();

            //var id = 0;

            UserPrincipalEx extpsdf = null;
            foreach (Principal p in results)
            {


                UserPrincipalEx extp = UserPrincipalEx.FindByIdentity(ctx, IdentityType.DistinguishedName, p.DistinguishedName);

                if (extp.SamAccountName == name)
                {
                    extp.Manager = pid;
                    
                    extp.Save();
                    extpsdf = extp;
                }

            }

                //up.EmailAddress = "emai1l1@gmail.com";


                //up.DisplayName = "user name11";

                //up.SetPassword("qaz11!@#WSX123");
                //up.SamAccountName = ""
                //up.Enabled = true;



                //up.SamAccountName = SamAccountName;





                // gluposti praq tuka

                //TempData["SamAccountName"] = SamAccountName;
                //return RedirectToAction("OrgChartGetManager", "User");


                //  ViewBag.SamAccountName = SamAccountName;

                //var user = new UserPrincipalEx(ctx);

                //UserPrincipalEx extp = new UserPrincipalEx(ctx);


                //extp.Manager = pid;
                //extp.DisplayName = "username1";
                //extp.EmailAddress = "emai1l@gmail.com";
                //extp.SetPassword("qaz1!@#WSX123");
                //extp.Enabled = true;
                //extp.SamAccountName = "emai1l";
                //extp.ExpirePasswordNow();


                //extp.Save();

                //extp.Manager = "CN=" + Manager + ",OU=TestOU,DC=ad,DC=balkangraph,DC=com";
                //extp.DisplayName
                //extp.Save();


                return Json(new { id = extpsdf.DistinguishedName });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
