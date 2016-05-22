using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RainIt.Business;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DeviceController : Controller
    {
        public IDeviceManager DeviceManager;

        public DeviceController(IDeviceManager deviceManager)
        {
            DeviceManager = deviceManager;
        }

        public ActionResult Index()
        {
            var allDevices = DeviceManager.GetAllDevices();
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
                var canAdd = DeviceManager.AddDevice(device);
                if (!canAdd.IsError)
                {
                    return RedirectToAction("Index", "Device", new { area = "Administration" });
                }
            }
            TempData["DeviceRegistrationModel"] = device;
            return RedirectToAction("Add", "Device", new { area = "Administration" });
        }
    }
}