using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Common.Util;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using Web.RainIt.Models;
using Web.RainIt.Models.Account;

namespace Web.RainIt.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class AccountController : Controller
    {
        public IAccountManager AccountManager;
        public IDeviceManager DeviceManager;

        public AccountController(IAccountManager accountManager, IDeviceManager deviceManager)
        {
            AccountManager = accountManager;
            DeviceManager = deviceManager;
        }
        
        public ActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            var registrationModel = TempData["RegistrationModel"] as RegistrationModel ?? new RegistrationModel();
            return View(registrationModel);
        }

        [System.Web.Mvc.HttpPost]
        public RedirectToRouteResult Register(RegistrationModel registrationModel)
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            var canRegister = new StatusMessage();
            if (ModelState.IsValid &&
                DeviceManager.IsDeviceAvailable(registrationModel.Registration.DeviceInfo.Identifier ?? Guid.Empty))
            {
                canRegister = AccountManager.Register(registrationModel.Registration);
            }
            else
            {
                canRegister.IsError = true;
                canRegister.Message = "An error occurred while validating your information";
            }
            TempData["StatusMessage"] = canRegister;
            if (!canRegister.IsError)
            {
                TempData["LoginModel"] = new Login()
                {
                    Username = registrationModel.Registration.User.Username
                };
                return RedirectToAction("Login", "Account");
            }
            
            TempData["RegistrationModel"] = registrationModel;
                return RedirectToAction("Register", "Account");
        }

        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            var loginModel = TempData["LoginModel"] as Login ?? new Login();
            return View(loginModel);
        }

        [System.Web.Mvc.HttpPost]
        public RedirectResult Login(Login login, string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated) return Redirect(Url.Action("Index", "Home"));
            var canAuthenticate = new StatusMessage();
            if (ModelState.IsValid)
            {
                canAuthenticate = AccountManager.Authenticate(login);
                if (!canAuthenticate.IsError)
                {
                    FormsAuthentication.SetAuthCookie(login.Username, false);
                }
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
            }
            TempData["StatusMessage"] = canAuthenticate;
            if(!canAuthenticate.IsError)
                return Redirect(Url.Action("Index", "Home", new {area = ""}));
            
            TempData["LoginModel"] = new Login(){ Username = login.Username};
            return Redirect(Url.Action("Login", "Account", new { area = "" }));
        }
    
        [System.Web.Http.Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public JsonResult IsUsernameAvailable(string username)
        {
            if (!String.IsNullOrWhiteSpace(username))
            {
                if(AccountManager.IsUsernameAvailable(username))
                    return Json(true, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = String.Format("The selected username is already in use.");
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailAvailable(string email)
        {
            if (!String.IsNullOrWhiteSpace(email))
            {
                if(AccountManager.IsEmailAvailable(email))
                    return Json(true, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = String.Format("The selected email is already in use.");
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsDeviceAvailable(Guid? identifier)
        {
            if (identifier.HasValue && identifier.Value != Guid.Empty)
            {
                if (DeviceManager.IsDeviceAvailable(identifier.Value))
                    return Json(true, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = String.Format("The selected device is not valid or may be already in use.");
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
    }
}