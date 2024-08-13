using SAP_Reports_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SAP_Reports_MVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(String Username, String Password, bool RememberMe)
        {
            UserTrainingxDBEntities db = new UserTrainingxDBEntities();

            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);


            if (Username == "manager" && Password == db.OUSRs.FirstOrDefault().PASSWORD4)
            {
                FormsAuthentication.SetAuthCookie(Username, RememberMe);

                return RedirectToAction("Index","Home");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Username or Password Incorrect! Try again.');</script>");
            }
        }
        // GET: /Login/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }
    }
}