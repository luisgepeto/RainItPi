using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        public IDeviceManager DeviceManager { get; set; }
        public DeviceController(IDeviceManager deviceManager)
        {
            DeviceManager = deviceManager;
        }

        [HttpGet]
        [Route("settings")]
        public IHttpActionResult GetSettings()
        {
            var currentSettings = DeviceManager.GetCurrentSettings();
            return Ok(currentSettings);
        }
    }
}
