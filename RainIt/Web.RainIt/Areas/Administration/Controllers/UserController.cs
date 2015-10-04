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
       
    }
}