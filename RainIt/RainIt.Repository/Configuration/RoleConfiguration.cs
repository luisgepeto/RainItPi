using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Role");
            HasKey(r => r.RoleId);
            Property(r => r.Name).IsRequired().HasMaxLength(20);
            Property(r => r.Description);
        }
    }
}
