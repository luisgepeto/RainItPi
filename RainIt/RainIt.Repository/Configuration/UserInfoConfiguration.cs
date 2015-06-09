
using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class UserInfoConfiguration : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".UserInfo");
            HasKey(u => u.UserId);
            Property(u => u.FirstName).IsRequired().HasMaxLength(20);
            Property(u => u.LastName).IsRequired().HasMaxLength(20);
            Property(u => u.BirthDate).IsRequired();
            Property(u => u.Gender).IsRequired();
        }
    }
}
