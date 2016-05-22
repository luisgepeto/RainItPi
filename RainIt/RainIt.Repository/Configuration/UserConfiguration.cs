
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
            HasRequired(u => u.Role)                
                .WithMany(r => r.Users)                
                .HasForeignKey(r => r.RoleId)
                .WillCascadeOnDelete(false);
            HasRequired(u => u.Password)
                .WithRequiredPrincipal(p => p.User)
                .WillCascadeOnDelete(true);
            HasRequired(u => u.UserInfo)
                .WithRequiredPrincipal(p => p.User)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Addresses)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Patterns)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Routines)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(true);
            HasMany(u => u.RoutinePatterns)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(true);
            HasMany(u => u.Devices)
                .WithOptional(r => r.User)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(true);
            HasRequired(u => u.UserSettings)
                .WithRequiredPrincipal(us => us.User)
                .WillCascadeOnDelete(true);
        }
    }
}
