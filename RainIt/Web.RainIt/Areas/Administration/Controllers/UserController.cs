using System.Web.Mvc;
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

        public PartialViewResult Details(int userId)
        {
            var userDetails = UserManager.GetDetails(userId);
            return PartialView(userDetails);
        }

        public PartialViewResult Devices(int userId)
        {
            var userDevices = UserManager.GetDevices(userId);
            return PartialView(userDevices);
        }
    }
}