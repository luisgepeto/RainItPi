using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class SampleRoutineConfiguration : EntityTypeConfiguration<SampleRoutine>
    {
        public SampleRoutineConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".SampleRoutine");
            HasKey(sr => sr.SampleRoutineId);
            Property(sr => sr.UpdateUTCDateTime).IsRequired();
        }
    }
}
