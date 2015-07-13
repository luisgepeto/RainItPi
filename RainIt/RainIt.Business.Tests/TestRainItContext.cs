using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Repository;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Repository;
using Address = RainIt.Domain.Repository.Address;
using User = RainIt.Domain.Repository.User;
using UserInfo = RainIt.Domain.Repository.UserInfo;

namespace RainIt.Business.Tests
{
    public class TestRainItContext : IRainItContext
    {
        public Domain.DTO.User CurrentUser
        {
            get
            {
                return new Domain.DTO.User()
                {
                    UserId = 1,
                    Username = "UnitTestUser"
                };
            }
        }

        public DeviceDTO CurrentDevice
        {
            get
            {
                return new DeviceDTO()
                {
                    Identifier = Guid.NewGuid(),
                    Serial = "123"
                };
            }
        }

        public DbSet<User> UserSet { get; set; }
        public DbSet<UserInfo> UserInfoSet{ get; set; }
        public DbSet<Address> AddressSet{ get; set; }
        public DbSet<Role> RoleSet{ get; set; }
        public DbSet<Password> PasswordSet{ get; set; }
        public DbSet<Pattern> PatternSet{ get; set; }
        public DbSet<Routine> RoutineSet{ get; set; }
        public DbSet<Device> DeviceSet{ get; set; }

        public DbSet<RoutinePattern> RoutinePatternSet{ get; set; }

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
                return
                    RoutineSet.Where(r => r.Devices.Select(d => d.DeviceInfo.DeviceId).Contains(CurrentDevice.DeviceId));
            }
            
        }

        public DbSet<SamplePattern> SamplePatternSet { get; set; }

        public DbSet<SampleRoutine> SampleRoutineSet { get; set; }
        }

        public int SaveChangesCount { get; private set; }

        public int SaveChanges()
        {
            SaveChangesCount++;
            return 1; 
        }
    }

    public class TestUserDbSet : MemoryDbSet<User>
    {
         public override User Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(u => u.UserId == (int)id);
        }
         public override User Add(User item)
        {
            if (item.UserId == 0)
            {
                item.UserId = this.Max(p => p.UserId) + 1;
            }
            return base.Add(item);
        }
    }

    public class TestUserInfoDbSet : MemoryDbSet<UserInfo>
    {
         public override UserInfo Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(ui => ui.UserId == (int)id);
        }
    }
    public class TestAddressDbSet : MemoryDbSet<Address>
    {
         public override Address Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(a => a.AddressId == (int)id);
        }
    }
    public class TestRoleDbSet : MemoryDbSet<Role>
    {
         public override Role Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(r => r.RoleId == (int)id);
        }
    }
    public class TestPasswordDbSet : MemoryDbSet<Password>
    {
         public override Password Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(p => p.UserId == (int)id);
        }
    }

    public class TestPatternDbSet : MemoryDbSet<Pattern>
    {
         public override Pattern Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(p => p.PatternId == (int)id);
        }
         public override Pattern Add(Pattern item)
        {
            if (item.PatternId == 0)
            {
                item.PatternId = this.Max(p => p.PatternId) + 1;
            }
            return base.Add(item);
        }
    }

    public class TestRoutineDbSet : MemoryDbSet<Routine>
    {
         public override Routine Find(params object[] keyValues)
        {
            var id = keyValues.Single();
            return this.SingleOrDefault(r => r.RoutineId == (int)id);
        }
    }
}
