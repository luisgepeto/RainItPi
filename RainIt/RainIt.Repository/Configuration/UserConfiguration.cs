
using System.Data.Entity.ModelConfiguration;
using RainIt.Domain.Repository;

namespace RainIt.Repository.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".User");
            HasKey(u => u.UserId);
            Property(u => u.Username).IsRequired().HasMaxLength(20);
            Property(u => u.Email).IsRequired().HasMaxLength(50);
            HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(ru =>
                {
                    ru.MapLeftKey("RoleId");
                    ru.MapRightKey("UserId");
                    ru.ToTable("RoleUser");
                });
            HasRequired(u => u.Password)
                .WithRequiredPrincipal(p => p.User)
                .WillCascadeOnDelete(true);
            HasRequired(u => u.UserInfo)
                .WithRequiredPrincipal(p => p.User)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Addresses)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);
            HasMany(u => u.Patterns)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Routines)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}
