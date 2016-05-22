using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Common.DependencyInjection;

namespace Web.RainIt.App_Start
{
    public class AutofacBootStrapper
    {
        public static void Run()
        {
            IContainer container = null;
            ContainerBuilder builder = RegisterDependencies();
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static ContainerBuilder RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RainItModule());
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof (MvcApplication).Assembly).PropertiesAutowired().InstancePerDependency();
            return builder;
        }
    }
}