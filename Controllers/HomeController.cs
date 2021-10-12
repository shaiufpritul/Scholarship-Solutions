using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(TempAdmin tempAdmin)
        {
            
            if (ModelState.IsValid)
            {
                if(tempAdmin.AdminName.Equals("Admin") && tempAdmin.AdminPassword.Equals("12345"))
                {
                    return RedirectToAction("AdminDashboard");
                }
                else
                {
                    ViewBag.LoginFailed = "Login Failed due to wrong username or password!";
                    return View();
                }
            }
            return View();
        }

        public ActionResult AdminDashboard()
        {
            return View(); 
        }
    }
}