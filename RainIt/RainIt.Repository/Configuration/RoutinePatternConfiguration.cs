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
        }
    }
}
