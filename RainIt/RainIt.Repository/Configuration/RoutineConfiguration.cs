using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class RoutineConfiguration : EntityTypeConfiguration<Routine>
    {
        public RoutineConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Routine");
            HasKey(r => r.RoutineId);
            Property(r => r.Name);
            Property(r => r.Description);
            Property(r => r.IsActive);

            //HasMany(r => r.RoutinePatterns)
            //    .WithRequired(rp => rp.Routine)
            //    .HasForeignKey(rp => rp.RoutineId)
            //    .WillCascadeOnDelete(true);
        }
    }
}

