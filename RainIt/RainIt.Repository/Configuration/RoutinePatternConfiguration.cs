using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class RoutinePatternConfiguration : EntityTypeConfiguration<RoutinePattern>
    {
        public RoutinePatternConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".RoutinePattern");
            HasKey(rp => rp.RoutinePatternId);

            HasOptional(rp => rp.Pattern)
                .WithMany(r => r.RoutinePatterns)
                .HasForeignKey(r => r.PatternId)
                .WillCascadeOnDelete(false);

            HasOptional(rp => rp.Routine)
                .WithMany(r => r.RoutinePatterns)
                .HasForeignKey(r => r.RoutineId)
                .WillCascadeOnDelete(false);
        }
    }
}
