
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using RainIt.Domain.Repository;

namespace RainIt.Interfaces.Repository
{
    public interface IRainItContext
    {
        Domain.DTO.User CurrentUser { get; }
        Domain.DTO.DeviceDTO CurrentDevice { get; }

        DbSet<User> UserSet { get; set; }
        DbSet<UserInfo> UserInfoSet { get; set; }
        DbSet<Address> AddressSet { get; set; }
        DbSet<Role> RoleSet { get; set; }
        DbSet<Password> PasswordSet { get; set; }
        DbSet<Pattern> PatternSet { get; set; }
        DbSet<Routine> RoutineSet { get; set; }
        DbSet<Device> DeviceSet { get; set; }
        DbSet<RoutinePattern> RoutinePatternSet { get; set; }
        IQueryable<Pattern> UserPatternSet { get; }
        IQueryable<Routine> UserRoutineSet { get; }
        IQueryable<Device> UserDeviceSet { get; }
        IQueryable<Routine> DeviceRoutineSet { get; }
        DbSet<SamplePattern> SamplePatternSet { get; }
        IQueryable<SamplePattern> DeviceSamplePatternSet { get; }
        DbSet<SampleRoutine> SampleRoutineSet { get; }
        IQueryable<SampleRoutine> DeviceSampleRoutineSet { get; }
        int SaveChanges();
        DbEntityEntry Entry(Routine currentRoutine);
    }
}
