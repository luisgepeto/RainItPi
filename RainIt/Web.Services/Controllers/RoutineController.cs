using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    [RoutePrefix("api/routine")]
    public class RoutineController : ApiController
    {
        public IRoutineManager RoutineManager { get; set; }

        public RoutineController(IRoutineManager routineManager)
        {
            RoutineManager = routineManager;
        }

        [HttpGet]
        [Route("allActive")]
        public IHttpActionResult GetAllActive()
        {
            var activeRoutines = RoutineManager.GetActiveRoutines();
            return Ok(activeRoutines);
        }

        [HttpGet]
        [Route("test")]
        public IHttpActionResult GetTest()
        {
            var testRoutine = RoutineManager.GetTestRoutine();
            return Ok(testRoutine);
        }
    }
}
