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
            Property(rp => rp.Repetitions);
            HasOptional(rp => rp.Pattern)
                .WithMany(p => p.RoutinePatterns)
                .HasForeignKey(p => p.PatternId)
                .WillCascadeOnDelete(false);

            HasOptional(rp => rp.Routine)
                .WithMany(r => r.RoutinePatterns)
                .HasForeignKey(r => r.RoutineId)
                .WillCascadeOnDelete(false);

            HasOptional(rp => rp.SampleRoutine)
                .WithMany(sr => sr.RoutinePatterns)
                .HasForeignKey(sr => sr.SampleRoutineId)
                .WillCascadeOnDelete(false);
        }
    }
}
