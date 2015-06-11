using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Web.Services.App_Start;

[assembly: OwinStartup(typeof(Web.Services.Startup))]

namespace Web.Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = AutofacBootStrapper.Run();
            app.UseAutofacMiddleware(container);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

    }
}