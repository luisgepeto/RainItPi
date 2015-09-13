using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class SettingsConfiguration : EntityTypeConfiguration<Settings>
    {
        public SettingsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Settings");
            HasKey(s => s.DeviceId);
            Property(s => s.MillisecondClockDelay).IsRequired();
            Property(s => s.MillisecondLatchDelay).IsRequired();
            Property(s => s.MinutesRefreshRate).IsRequired();
        }
    }
}
