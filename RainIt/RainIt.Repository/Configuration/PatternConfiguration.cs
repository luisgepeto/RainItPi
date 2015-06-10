using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class PatternConfiguration : EntityTypeConfiguration<Pattern>
    {
        public PatternConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Pattern");
            HasKey(p => p.PatternId);
            Property(p => p.Name).IsRequired().HasMaxLength(50);
            Property(p => p.Description);
            Property(p => p.FileType).IsRequired().HasMaxLength(50);
            Property(p => p.BytesFileSize).IsRequired();
            Property(p => p.Width).IsRequired();
            Property(p => p.Height).IsRequired();
            Property(p => p.Path).IsRequired();     
      
            //HasMany(r => r.RoutinePatterns)
            //    .WithRequired(rp => rp.Pattern)
            //    .HasForeignKey(rp => rp.PatternId)
            //    .WillCascadeOnDelete(true);    
        }
    }
}
