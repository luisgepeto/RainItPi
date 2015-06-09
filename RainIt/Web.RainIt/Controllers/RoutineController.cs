using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Controllers
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
            return View("Add", routine);
        }
        
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(int routineId, List<int> patternIdList)
        {
            StatusMessage canAdd = null;
            var routineList = new RoutineList()
            {
                RoutineId = routineId,
                RoutinePatterns = patternIdList.Select(p => new RoutinePatternList()
                {
                    PatternId = p
                }).ToList(),
                Description = "some description",
                Name = "some name"
            };
            if (routineId == 0)
            {
                canAdd = RoutineManager.AddUserRoutine(routineList);
            }
            if (routineId > 0)
            {
                canAdd = RoutineManager.UpdateUserRoutine(routineList);
            }
            return View();
        }
    }
}