namespace Newsweek.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> identityUserRole)
        {
            identityUserRole.Property<string>("UserId");
            
            identityUserRole.Property<string>("RoleId");
            
            identityUserRole.HasKey("UserId", "RoleId");
 
            identityUserRole.HasIndex("RoleId");
 
            identityUserRole.ToTable("AspNetUserRoles");
        }
    }
}