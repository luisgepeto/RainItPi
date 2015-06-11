using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Common.DependencyInjection;


namespace Web.Services.App_Start
{
    public class AutofacBootStrapper
    {
        public static IContainer Run()
        {
            IContainer container = null;
            ContainerBuilder builder = RegisterDependencies();
            container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        private static ContainerBuilder RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => HttpContext.Current.Request).As<HttpRequest>();
            builder.RegisterModule(new RainItServicesModule());
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired().InstancePerRequest();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            return builder;
        }
    }
}