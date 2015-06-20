using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using RainIt.Business;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;
using RainIt.Repository;
using Web.Security.Business;
using Web.Security.Interfaces;

namespace Common.DependencyInjection
{
    public class RainItServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
            builder.RegisterType<TokenManager>()
                .As<ITokenManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.RegisterType<PatternManager>()
                .As<IPatternManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.RegisterType<AccountManager>()
                .As<IAccountManager>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.RegisterType<DeviceManager>()
               .As<IDeviceManager>()
               .PropertiesAutowired()
               .InstancePerLifetimeScope();
        }
    }
}
