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
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        public IDeviceManager DeviceManager { get; set; }
        public ITokenManager TokenManager { get; set; }

        public AccountController(IDeviceManager deviceManager, ITokenManager tokenManager)
        {
            DeviceManager = deviceManager;
            TokenManager = tokenManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(string serial)
        {
            var authResult = new AuthenticationResult();
            if (ModelState.IsValid)
            {
                var canAuthenticate = DeviceManager.ValidateDevice(serial);
                if (canAuthenticate)
                {
                    authResult.LoginStatus = LoginStatus.ValidUser;
                    authResult.TokenExpirationUtcTime = DateTime.UtcNow.AddDays(7);
                    authResult.SecurityToken = TokenManager.CreateJwtToken(serial);
                }
            }
            return Ok(authResult);
        }
    }
}
