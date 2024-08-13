using SAP_Reports_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAP_Reports_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ChangePassword()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public string ChangePassword(String oldPassword, string newPassword, string confirmPassword)
        {
            UserTrainingxDBEntities db = new UserTrainingxDBEntities();

            if (newPassword != "" && newPassword == confirmPassword)
            {
                if (oldPassword == db.OUSRs.FirstOrDefault().PASSWORD4)
                {
                    db.OUSRs.FirstOrDefault().PASSWORD4 = newPassword;
                    db.SaveChanges();
                    return "Password Changed Successfully.";
                }
                else
                {
                    return "Error! Wrong Old Password!";
                }
            }
            else
            {
                 return "Error! New Password don't match!";
            }
        }


    }
}