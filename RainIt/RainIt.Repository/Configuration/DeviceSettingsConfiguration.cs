using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class DeviceSettingsConfiguration : EntityTypeConfiguration<DeviceSettings>
    {
        public DeviceSettingsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DeviceSettings");
            HasKey(s => s.DeviceId);
            Property(s => s.MillisecondClockDelay).IsRequired();
            Property(s => s.MillisecondLatchDelay).IsRequired();
            Property(s => s.MinutesRefreshRate).IsRequired();
        }
    }
}
