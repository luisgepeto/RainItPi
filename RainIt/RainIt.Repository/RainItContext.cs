using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Repository;
using RainIt.Repository.Configuration;

namespace RainIt.Repository
{
    public class RainItContext : DbContext, IRainItContext
    {
        public RainItContext() : base("DevRainItDb")
        {
            Database.Log = Console.Write;
        }

        public Domain.DTO.User CurrentUser
        {
            get
            {
                var username = HttpContext.Current.User.Identity.Name;
                var userId = UserSet.Single(u => u.Username == username).UserId;
                return new Domain.DTO.User()
                {
                    UserId = userId,
                    Username = username
                };
            }
        }

        public DbSet<User> UserSet { get; set; }
        public DbSet<UserInfo> UserInfoSet { get; set; }
        public DbSet<Address> AddressSet { get; set; }
        public DbSet<Role> RoleSet { get; set; }
        public DbSet<Password> PasswordSet { get; set; }
        public DbSet<Pattern> PatternSet { get; set; }
        public DbSet<Routine> RoutineSet { get; set; }

        public DbSet<RoutinePattern> RoutinePatternSet { get; set; }
        public DbSet<Device> DeviceSet { get; set; }
        public DbSet<DeviceInfo> DeviceInfoSet { get; set; }
        //public DbSet<DeviceCredential> DeviceCredentialSet { get; set; }

        public IQueryable<Pattern> UserPatternSet
        {
            get { return PatternSet.Where(p => p.UserId == CurrentUser.UserId); }
        }
        public IQueryable<Routine> UserRoutineSet
        {
            get { return RoutineSet.Where(p => p.UserId == CurrentUser.UserId); }
        }
        public IQueryable<Device> UserDeviceSet
        {
            get { return DeviceSet.Where(p => p.UserId == CurrentUser.UserId); }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new PasswordConfiguration());
            modelBuilder.Configurations.Add(new PatternConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new RoutineConfiguration());
            modelBuilder.Configurations.Add(new RoutinePatternConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserInfoConfiguration());
            modelBuilder.Configurations.Add(new DeviceConfiguration());
            modelBuilder.Configurations.Add(new DeviceInfoConfiguration());
            modelBuilder.Configurations.Add(new DeviceCredentialConfiguration());
        }
    }
}

