using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class SamplePatternConfiguration : EntityTypeConfiguration<SamplePattern>
    {
        public SamplePatternConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".SamplePattern");
            HasKey(sp => sp.SamplePatternId);
            Property(sp => sp.UpdateUTCDateTime).IsRequired();
            Property(sp => sp.Base64Image).HasMaxLength(null).IsRequired();
        }
    }
}
