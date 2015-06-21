using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Web.Infrastructure.Attributes
{
    public class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        private int _timespan;
        private int _clientTimeSpan;
        private bool _anonymousOnly;
        private string _cachekey;
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        public WebApiOutputCacheAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
            _anonymousOnly = anonymousOnly;
        }

        private bool IsCacheable(HttpActionContext ac)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (_anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (ac.Request.Method == HttpMethod.Get) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }
        private CacheControlHeaderValue SetClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue();
            cachecontrol.MaxAge = TimeSpan.FromSeconds(_clientTimeSpan);
            cachecontrol.MustRevalidate = true;
            return cachecontrol;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                if (IsCacheable(actionContext))
                {
                    _cachekey = BuildCacheKey(actionContext);
                    if (WebApiCache.Contains(_cachekey))
                    {
                        var val = (string)WebApiCache.Get(_cachekey);
                        if (val != null)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse();
                            actionContext.Response.Content = new StringContent(val);
                            var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct");
                            if (contenttype == null)
                                contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                            actionContext.Response.Content.Headers.ContentType = contenttype;
                            actionContext.Response.Headers.CacheControl = SetClientCache();
                            return;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        private static string BuildCacheKey(HttpActionContext actionContext)
        {
            var cacheVariable = String.Join("_", actionContext.ActionArguments.Select(aa => aa.Key + "^" + aa.Value));
            return string.Join(":", new string[] { actionContext.Request.RequestUri.AbsolutePath, cacheVariable });
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (_cachekey != null)
            {
                if (!(WebApiCache.Contains(_cachekey)))
                {
                    var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                    WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(_timespan));
                    WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
                }
            }
            if (IsCacheable(actionExecutedContext.ActionContext))
                actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }
    }
}
