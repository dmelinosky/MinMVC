﻿using System.Diagnostics;
using System.Web.Mvc;

namespace ClientResourceFullCS.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(TestInjection test)
        {
            Debug.Assert(test != null);
        }

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
    }
}