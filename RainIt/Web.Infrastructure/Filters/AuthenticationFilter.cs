using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using RainIt.Interfaces.Repository;
using Web.Security.Interfaces;

namespace Web.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticationFilter : AuthorizeAttribute
    {
        private const string BearerAuthorizationResponseHeader = "Authorization";
        private const string BearerAuthorizationResponseHeaderValue = "Bearer";
        
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!SkipAuthorization(actionContext))
            {
                var principal = AuthenticateRequest(actionContext);
                if (principal == null)
                {
                    Challenge(actionContext);
                    return;
                }

                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
            base.OnAuthorization(actionContext);
        }

        protected virtual ClaimsPrincipal AuthenticateRequest(HttpActionContext actionContext)
        {
            string authorizationHeader = null;
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization != null)
            {
                authorizationHeader = authorization.Parameter;
            }

            if (String.IsNullOrWhiteSpace(authorizationHeader)) return null;

            var jwtEncodedString = authorizationHeader;
            var tokenService = (ITokenManager)actionContext.Request.GetDependencyScope().GetService(typeof(ITokenManager));

            return tokenService.ValidateToken(jwtEncodedString);

        }

        void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }

}
