using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class ConversionParameterConfiguration : EntityTypeConfiguration<ConversionParameter>
    {
        public ConversionParameterConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ConversionParameter");
            HasKey(cp => cp.PatternId);
            Property(cp => cp.RWeight).IsRequired();
            Property(cp => cp.GWeight).IsRequired();
            Property(cp => cp.BWeight).IsRequired();
            Property(cp => cp.ThresholdPercentage).IsRequired();
            Property(cp => cp.IsInverted).IsRequired();
        }
    }
}
