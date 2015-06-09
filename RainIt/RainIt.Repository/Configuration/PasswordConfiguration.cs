using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class PasswordConfiguration : EntityTypeConfiguration<Password>
    {
        public PasswordConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Password");
            HasKey(a => a.UserId);
            Property(a => a.Salt).IsRequired();
            Property(a => a.Hash).IsRequired();
        }
    }
}
