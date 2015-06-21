using System.Collections.Generic;
using System.Linq;
using RainIt.Domain.Repository;

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
        }
    }
}
