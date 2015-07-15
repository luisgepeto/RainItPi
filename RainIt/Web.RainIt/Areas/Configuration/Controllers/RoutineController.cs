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
            ViewBag.ConstraintParameters = RoutineManager.GetUploadConstraintParameters();
            return View("Add",routine);
        }
        
        public ActionResult Add()
        {
            ViewBag.ConstraintParameters = RoutineManager.GetUploadConstraintParameters();
            return View();
        }

        [HttpPost]
        public JsonResult Add(RoutineUploadModel routineUploadModel)
        {
            StatusMessage canAdd = null;
            if (routineUploadModel.RoutineId == 0)
            {
                canAdd = RoutineManager.AddUserRoutine(routineUploadModel);
            }
            if (routineUploadModel.RoutineId > 0)
            {
                canAdd = RoutineManager.UpdateUserRoutine(routineUploadModel);
            }
            return Json(Url.Action("Index", "Routine", new {area = "Configuration"}));
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Test(RoutineUploadModel routineUploadModel)
        {
            StatusMessage canSet = new StatusMessage();
            if (routineUploadModel.DeviceIdentifierList == null || !routineUploadModel.DeviceIdentifierList.Any())
                canSet = StatusMessage.WriteError("No device was selected");
            if (!canSet.IsError)
            {
                canSet = RoutineManager.SetToTest(routineUploadModel);
            }
            return Json(new { canSet }, JsonRequestBehavior.DenyGet);
        }

    }
}