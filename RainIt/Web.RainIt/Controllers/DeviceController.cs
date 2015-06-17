using System.Linq;
using System.Web.Mvc;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Controllers
{
    public class DeviceController : Controller
    {
        public IDeviceManager DeviceManager;

        public DeviceController(IDeviceManager deviceManager)
        {
            DeviceManager = deviceManager;
        }

        public ActionResult Index()
        {
            var allDevices = DeviceManager.GetUserDevices();
            return View(allDevices);
        }

        public ActionResult Add()
        {
            var deviceRegistrationModel = TempData["DeviceRegistrationModel"] as DeviceDTO ?? new DeviceDTO();
            return View(deviceRegistrationModel);
        }

        [HttpPost]
        public RedirectToRouteResult Add(DeviceDTO device)
        {
            if (ModelState.IsValid)
            {
                var canAdd = DeviceManager.AddUserDevice(device);
                if (!canAdd.IsError)
                {
                    return RedirectToAction("Index");
                }
            }
            TempData["DeviceRegistrationModel"] = device;
            return RedirectToAction("Add");
        }

        [HttpGet]
        public JsonResult GetUserDevices()
        {
            var allDevices = DeviceManager.GetUserDevices();
            return Json(new {allDevices}, JsonRequestBehavior.AllowGet);
        }
    }
}