using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class DeviceInfoConfiguration : EntityTypeConfiguration<DeviceInfo>
    {
        public DeviceInfoConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DeviceInfo");
            HasKey(d => d.DeviceId);
            Property(d => d.Identifier).IsRequired();
            Property(d => d.Serial).IsRequired();
            Property(d => d.ActivatedUTCDate).IsOptional();
        }
    }
}
