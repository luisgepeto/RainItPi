using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class AddressConfiguration : EntityTypeConfiguration<Address>
    {
        public AddressConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Address");
            HasKey(a => a.AddressId);
            Property(a => a.Country).IsRequired().HasMaxLength(50);
            Property(a => a.State).IsRequired().HasMaxLength(50);
            Property(a => a.City).IsRequired().HasMaxLength(50);
            Property(a => a.AddressLine1).IsRequired().HasMaxLength(50);
            Property(a => a.AddressLine2).IsOptional().HasMaxLength(50);
            Property(a => a.PhoneNumber).IsOptional().HasMaxLength(20);
        }
    }
}
