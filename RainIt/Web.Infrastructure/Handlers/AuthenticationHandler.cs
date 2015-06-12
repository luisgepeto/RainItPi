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
                // Create the 401 response
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Headers.Add(BearerAuthorizationResponseHeader, BearerAuthorizationResponseHeaderValue);
               
                // Create a task that does not contain a delegate for the inner handlers
                var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
                taskCompletionSource.SetResult(response);   
                return taskCompletionSource.Task;
            }

            // Continue on happy path for validated request
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
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            if (authorization != null)
            {
                authorizationHeader = authorization.Parameter;
            }
            if (String.IsNullOrWhiteSpace(authorizationHeader))
            {
                return null;
            }

            var jwtEncodedString = authorizationHeader;
            var rainItContext = (IRainItContext)request.GetDependencyScope().GetService(typeof(IRainItContext));
            var tokenService = (ITokenManager)request.GetDependencyScope().GetService(typeof(ITokenManager));

            return tokenService.ValidateToken(jwtEncodedString);
            
        }
    }
}
