using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using RainIt.Interfaces.Repository;
using Web.Security.Interfaces;

namespace Web.Infrastructure.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private const string BearerAuthorizationResponseHeader = "Authorization";
        private const string BearerAuthorizationResponseHeaderValue = "Bearer";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                           CancellationToken cancellationToken)
        {
            var principal = AuthenticateRequest(request);
            
            if (principal == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Headers.Add(BearerAuthorizationResponseHeader, BearerAuthorizationResponseHeaderValue);
               
                var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
                taskCompletionSource.SetResult(response);   
                return taskCompletionSource.Task;
            }

            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return base.SendAsync(request, cancellationToken);
            
        }

        protected virtual ClaimsPrincipal AuthenticateRequest(HttpRequestMessage request)
        {
            string authorizationHeader = null;
            var authorization = request.Headers.Authorization;
            if (authorization != null)
            {
                authorizationHeader = authorization.Parameter;
            }

            if (String.IsNullOrWhiteSpace(authorizationHeader)) return null;
            
            var jwtEncodedString = authorizationHeader;
            var tokenService = (ITokenManager)request.GetDependencyScope().GetService(typeof(ITokenManager));

            return tokenService.ValidateToken(jwtEncodedString);
            
        }
    }
}
