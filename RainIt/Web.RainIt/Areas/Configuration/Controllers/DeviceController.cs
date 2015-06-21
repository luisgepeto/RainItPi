using System.Web.Mvc;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Areas.Configuration.Controllers
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
                    return RedirectToAction("Index", "Device", new{area = "Configuration"});
                }
            }
            TempData["DeviceRegistrationModel"] = device;
            return RedirectToAction("Add", "Device", new { area = "Configuration" });
        }

        public PartialViewResult Multiselect()
        {
            var allDevices = DeviceManager.GetUserDevices();
            return PartialView("_Multiselect",allDevices);
        }
    }
}