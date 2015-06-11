using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class DeviceConfiguration : EntityTypeConfiguration<Device>
    {
        public DeviceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Device");
            HasKey(d => d.DeviceId);
            HasRequired(d => d.DeviceCredential)
                .WithRequiredPrincipal(p => p.Device)
                .WillCascadeOnDelete(true);
            HasRequired(d => d.DeviceInfo)
                .WithRequiredPrincipal(p => p.Device)
                .WillCascadeOnDelete(true);
            HasOptional(d => d.Routine)
                .WithMany(r => r.Devices)
                .HasForeignKey(d => d.RoutineId)
                .WillCascadeOnDelete(false);
        }
    }
}
