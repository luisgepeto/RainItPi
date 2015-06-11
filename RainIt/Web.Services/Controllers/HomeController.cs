using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Services.Controllers
{
    public class HomeController : Controller
    {
        public RedirectToRouteResult Index()
        {
            ViewBag.Title = "Home Page";

            return RedirectToAction("Index", "Help");
        }
    }
}
