using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.RainIt.Areas.Administration.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class DeviceController : Controller
    {
        // GET: Administration/Device
        public ActionResult Index()
        {
            return View();
        }
    }
}