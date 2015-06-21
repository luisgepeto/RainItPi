using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using RainIt.Domain.Repository;
using Web.Security.Interfaces;
using System.Web.Http;
using Web.Security.Business;

namespace RainIt.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<RainItContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(RainItContext context)
        {
            context.RoleSet.AddOrUpdate(r => r.RoleId, new Role
            {
                RoleId = 1,
                Description = "Administrator Role",
                Name = "administrator"
            });
            context.RoleSet.AddOrUpdate(r => r.RoleId, new Role
            {
                RoleId = 2,
                Description = "Customer Role",
                Name = "customer"
            });

            context.UserSet.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 1,
                Username = "luisgepeto",
                Email = "luisgepeto@gmail.com",
                Roles = context.RoleSet.Where(rs => rs.Name=="administrator").ToList()
            });

            context.UserInfoSet.AddOrUpdate(ui => ui.UserId, new UserInfo()
            {
                UserId = 1,
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
                UserId = 1,
                Salt = currentSalt,
                Hash = currentHash
            });
        }
    }
}
