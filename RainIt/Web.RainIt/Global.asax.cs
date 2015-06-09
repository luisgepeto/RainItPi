using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using RainIt.Business;
using RainIt.Repository;
using RainIt.Repository.Migrations;
using Web.RainIt.App_Start;
using Configuration = RainIt.Repository.Migrations.Configuration;

namespace Web.RainIt
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacBootStrapper.Run();

            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                var configuration = new Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                try
                {

                    string username =
                        FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                    List<string> roles;
                    using (RainItContext entities = new RainItContext())
                    {
                        var accountManager = new AccountManager(entities);
                        roles = accountManager.GetRolesFor(username);
                    }

                    HttpContext.Current.User =
                        new System.Security.Principal.GenericPrincipal(
                            new System.Security.Principal.GenericIdentity(username, "Forms"), roles.ToArray());
                }
                catch (Exception ex)
                {
                    //TODO log exception
                }
            }
        }
    }
}
