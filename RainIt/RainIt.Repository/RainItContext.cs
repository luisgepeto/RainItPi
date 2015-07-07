using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading;
using System.Web;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Repository;
using RainIt.Repository.Configuration;
using Address = RainIt.Domain.Repository.Address;
using User = RainIt.Domain.Repository.User;
using UserInfo = RainIt.Domain.Repository.UserInfo;

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

        public DeviceDTO CurrentDevice
        {
            get
            {
                var serial = HttpContext.Current.User.Identity.Name;
                var identifier = Guid.Empty;

                var hashClaim = ((ClaimsPrincipal)HttpContext.Current.User).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash);
                if (hashClaim != null)
                {
                    var claimsIdentifier = hashClaim.Value;
                    Guid.TryParse(claimsIdentifier, out identifier);
                }

                var deviceId = DeviceSet.Single(d => d.DeviceInfo.Serial == serial && d.DeviceInfo.Identifier == identifier).DeviceId;
                
                return new DeviceDTO()
                {
                    DeviceId = deviceId,
                    Identifier = identifier,
                    Serial = serial
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
        public DbSet<SamplePattern> SamplePatternSet { get; set; }

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

        public IQueryable<Routine> DeviceRoutineSet
        {
            get
            {
                return RoutineSet.Where(r => r.Devices.Select(d => d.DeviceId).Contains(CurrentDevice.DeviceId));
                //return DeviceSet.Where(d => d.DeviceId == CurrentDevice.DeviceId).Select(d => d.Routine);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

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
            modelBuilder.Configurations.Add(new ConversionParameterConfiguration());
            modelBuilder.Configurations.Add(new SamplePatternConfiguration());
        }
    }
}

