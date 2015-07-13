using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Areas.Configuration.Controllers
{
    public class RoutineController : Controller
    {
        public IRoutineManager RoutineManager;

        public RoutineController(IRoutineManager routineManager)
        {
            RoutineManager = routineManager;
        }
        public ActionResult Index()
        {
            var allRoutines = RoutineManager.GetUserRoutines();
            return View(allRoutines);
        }
        public ActionResult Edit(int routineId)
        {
            var routine = RoutineManager.GetUserRoutine(routineId);
            return View("Add",routine);
        }
        
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(int routineId, List<int> patternIdList, List<Guid> deviceGuidList)
        {
            StatusMessage canAdd = null;

            var routineList = new RoutineDTO()
            {
                RoutineId = routineId,
                RoutinePatternDTOs = patternIdList.Select(p => new RoutinePatternDTO()
                {
                    PatternDTO = new PatternDTO(){ PatternId = p }
                }).ToList(),
                Description = "some description",
                Name = "some name",
                DeviceDTOs = deviceGuidList == null
                    ? new List<DeviceDTO>()
                    : deviceGuidList.Select(d => new DeviceDTO()
                    {
                        Identifier = d
                    }).ToList()
            };

            if (routineId == 0)
            {
                canAdd = RoutineManager.AddUserRoutine(routineList);
            }
            if (routineId > 0)
            {
                canAdd = RoutineManager.UpdateUserRoutine(routineList);
            }
            return Json(Url.Action("Index", "Routine", new {area = "Configuration"}));
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Test(List<int> patternIdList, List<Guid> deviceIdentifierList)
        {
            StatusMessage canSet = new StatusMessage();
            if (deviceIdentifierList == null || !deviceIdentifierList.Any())
                canSet = StatusMessage.WriteError("No device was selected");
            if (!canSet.IsError)
            {
                canSet = RoutineManager.SetToTest(patternIdList, deviceIdentifierList);
            }
            return Json(new { canSet }, JsonRequestBehavior.DenyGet);
        }

    }
}