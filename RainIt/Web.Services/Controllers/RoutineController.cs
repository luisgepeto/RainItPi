using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    public class RoutineController : ApiController
    {
        public IRoutineManager RoutineManager { get; set; }

        public RoutineController(IRoutineManager routineManager)
        {
            RoutineManager = routineManager;
        }

        [HttpGet]
        public IHttpActionResult AllActive()
        {
            var activeRoutine = RoutineManager.GetActiveRoutines();
            return Ok(activeRoutine);
        }
    }
}
