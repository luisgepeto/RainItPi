﻿using System;
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

        public AccountController(IAccountManager accountManager)
        {
            AccountManager = accountManager;
        }
        
        public ActionResult Register()
        {
            var registrationModel = TempData["RegistrationModel"] as RegistrationModel ?? new RegistrationModel();
            return View(registrationModel);
        }

        [System.Web.Mvc.HttpPost]
        public RedirectToRouteResult Register(RegistrationModel registrationModel)
        {
            var canRegister = new StatusMessage();
            if (ModelState.IsValid)
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
            var loginModel = TempData["LoginModel"] as Login ?? new Login();
            return View(loginModel);
        }

        [System.Web.Mvc.HttpPost]
        public RedirectResult Login(Login login, string returnUrl)
        {
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
    }
}