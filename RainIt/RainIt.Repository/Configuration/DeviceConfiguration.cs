﻿using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class DeviceConfiguration : EntityTypeConfiguration<Device>
    {
        public DeviceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Device");
            HasKey(d => d.DeviceId);
            HasRequired(d => d.DeviceInfo)
                .WithRequiredPrincipal(p => p.Device)
                .WillCascadeOnDelete(true);
            HasRequired(d => d.DeviceSettings)
                .WithRequiredPrincipal(s => s.Device)
                .WillCascadeOnDelete(true);
            HasMany(d => d.Routines)
                .WithMany(r => r.Devices)
                .Map(cs =>
                {
                    cs.MapLeftKey("DeviceId");
                    cs.MapRightKey("RoutineId");
                    cs.ToTable("DeviceRoutine");
                });
            HasOptional(d => d.SamplePattern)
                .WithRequired(sp=> sp.Device)
                .WillCascadeOnDelete(true);
            HasOptional(d => d.SampleRoutine)
                .WithRequired(sr=> sr.Device)
                .WillCascadeOnDelete(true);
            Property(d => d.Name).HasMaxLength(50);
        }
    }
}
