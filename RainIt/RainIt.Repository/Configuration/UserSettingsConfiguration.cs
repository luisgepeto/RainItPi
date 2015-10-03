using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class UserSettingsConfiguration : EntityTypeConfiguration<UserSettings>
    {
        public UserSettingsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".UserSettings");
            HasKey(us => us.UserId);
            Property(us => us.MaxPatternByteCount).IsRequired();
            Property(us => us.MaxPatternPixelHeight).IsRequired();
            Property(us => us.MaxPatternPixelWidth).IsRequired();
            Property(us => us.MaxPatternCountPerRoutine).IsRequired();
            Property(us => us.MaxNumberOfRepetitionsPerPattern).IsRequired();
        }
    }
}
