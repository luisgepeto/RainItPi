using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;
using Web.Security.Domain;
using Web.Security.Interfaces;

namespace Web.Services.Controllers
{
    public class AccountController : ApiController
    {
        public IAccountManager AccountManager { get; set; }
        public ITokenManager TokenManager { get; set; } 

        public AccountController(IAccountManager accountManager, ITokenManager tokenManager)
        {
            AccountManager = accountManager;
            TokenManager = tokenManager;
        }

        [HttpPost]
        public IHttpActionResult Login(Login login)
        {
            var authResult = new AuthenticationResult();
            if (ModelState.IsValid)
            {
                var canAuthenticate = AccountManager.Authenticate(login);
                if (!canAuthenticate.IsError)
                {
                    authResult.LoginStatus = LoginStatus.ValidUser;
                    authResult.TokenExpirationUtcTime = DateTime.UtcNow.AddDays(7);
                    authResult.SecurityToken = TokenManager.CreateJwtToken(login.Username);
                }
            }
            return Ok(authResult);
        }
    }
}
