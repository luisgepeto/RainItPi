﻿using System.Web;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;
using Common.DependencyInjection;
using Web.Services;

namespace Web.Services.App_Start
{
    public class AutofacBootStrapper
    {
        public static void Run()
        {
            IContainer container = null;
            ContainerBuilder builder = RegisterDependencies();
            container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static ContainerBuilder RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => HttpContext.Current.Request).As<HttpRequest>();
            builder.RegisterModule(new RainItModule());
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired().InstancePerDependency();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            return builder;
        }
    }
}