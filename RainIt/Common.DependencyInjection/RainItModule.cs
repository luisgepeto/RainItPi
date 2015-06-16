using Autofac;
using ImageProcessing.Business;
using ImageProcessing.Business.Interfaces;
using RainIt.Business;
using RainIt.Interfaces;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;
using RainIt.Repository;

namespace Common.DependencyInjection
{
    public class RainItModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var connString = ConfigurationManager.ConnectionStrings["BigODatabase"].ConnectionString;
            builder.Register(c => new RainItContext());
            builder.Register(c => new AzureCloudContext());

            builder.RegisterType<RainItContext>()
                .As<IRainItContext>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterType<AzureCloudContext>()
                .As<IAzureCloudContext>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            //TODO Here we will register the dependencies for the respective managers
            builder.RegisterType<AccountManager>()
                .As<IAccountManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterType<PatternManager>()
                .As<IPatternManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoutineManager>()
                .As<IRoutineManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterType<ImageManager>()
                .As<IImageManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }
    }
}
