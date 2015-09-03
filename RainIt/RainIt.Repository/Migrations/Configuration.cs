using System;
using System.Data.Entity.Migrations;
using System.Linq;
using RainIt.Domain.Repository;
using Web.Security.Business;
using Web.Security.Interfaces;

namespace RainIt.Repository.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<RainItContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        public static void SeedMethod(RainItContext context)
        {
            var administratorRole = context.RoleSet.SingleOrDefault(r => r.Name == "administrator");
            context.RoleSet.AddOrUpdate(r => r.RoleId, new Role
            {
                RoleId =  administratorRole != null ? administratorRole.RoleId : 1,
                Description = "Administrator Role",
                Name = "administrator"
            });
            var customerRole = context.RoleSet.SingleOrDefault(r => r.Name == "customer");
            context.RoleSet.AddOrUpdate(r => r.RoleId, new Role
            {
                RoleId = customerRole != null ? customerRole.RoleId : 2,
                Description = "Customer Role",
                Name = "customer"
            });

            var role = context.RoleSet.SingleOrDefault(r => r.Name == "administrator");
            var user = context.UserSet.SingleOrDefault(r => r.Username == "luisgepeto");
            context.UserSet.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = user != null ? user.UserId : 1,
                Username = "luisgepeto",
                Email = "luisgepeto@gmail.com",
                RoleId = role != null ? role.RoleId : 1
            });

            context.UserInfoSet.AddOrUpdate(ui => ui.UserId, new UserInfo()
            {
                UserId = user != null ? user.UserId : 1,
                FirstName = "Luis",
                LastName = "Becerril",
                Gender = "Male",
                BirthDate = new DateTime(1991, 7, 26)
            });

            var cryptoServiceManager = (ICryptoServiceManager) new CryptoServiceManager();
            var currentSalt = cryptoServiceManager.CreateRandomSalt();
            var concatenatedPass = currentSalt + "makiburtub";
            string currentHash = cryptoServiceManager.GetHashFrom(concatenatedPass);

            context.PasswordSet.AddOrUpdate(p => p.UserId, new Password()
            {
                UserId = user != null ? user.UserId : 1,
               Salt = currentSalt,
                Hash = currentHash
            });
        }
        protected override void Seed(RainItContext context)
        {
            SeedMethod(context);
        }
    }
}
