using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.RainIt.Areas.Image.Controllers
{
    public class ConversionController : Controller
    {
        // GET: Image/Conversion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(IEnumerable<HttpPostedFileBase> files)
        {
            return View(files);
        }
    }
}