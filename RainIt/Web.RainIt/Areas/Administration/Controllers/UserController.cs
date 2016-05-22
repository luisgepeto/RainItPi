using System.Web.Mvc;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.RainIt.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        public IUserManager UserManager { get; set; }

        public UserController(IUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index()
        {
            var allUsers = UserManager.GetAllUsers();
            return View(allUsers);
        }

        public ActionResult Details(int userId)
        {
            var userDetails = UserManager.GetInformation(userId);
            return View(userDetails);
        }

        public PartialViewResult Information(int userId)
        {
            var userInformation = UserManager.GetInformation(userId);
            return PartialView("_Information", userInformation);
        }

        public PartialViewResult EditInformation(UserDTO user)
        {
            return PartialView("_EditInformation", user);
        }

        [HttpPost]
        public JsonResult SaveInformation(UserDTO user)
        {
            var canEdit = UserManager.EditInformation(user);
            return Json(new { canEdit }, JsonRequestBehavior.DenyGet);
        }

        public PartialViewResult Settings(int userId)
        {
            var userSettings = UserManager.GetSettings(userId);
            return PartialView("_Settings", userSettings);
        }

        public PartialViewResult Devices(int userId)
        {
            var userDevices = UserManager.GetDevices(userId);
            return PartialView("_Devices",userDevices);
        }
    }
}