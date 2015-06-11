using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class DeviceCredentialConfiguration : EntityTypeConfiguration<DeviceCredential>
    {
        public DeviceCredentialConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DeviceCredential");
            HasKey(d => d.DeviceId);
            Property(d => d.Salt).IsRequired();
            Property(d => d.Hash).IsRequired();
        }
    }
}
