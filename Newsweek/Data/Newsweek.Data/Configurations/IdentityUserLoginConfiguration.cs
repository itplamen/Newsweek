namespace Newsweek.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> identityUserLogin)
        {
            identityUserLogin.Property<string>("LoginProvider");

            identityUserLogin.Property<string>("ProviderKey");

            identityUserLogin.Property<string>("ProviderDisplayName");

            identityUserLogin.Property<string>("UserId").IsRequired();

            identityUserLogin.HasKey("LoginProvider", "ProviderKey");

            identityUserLogin.HasIndex("UserId");

            identityUserLogin.ToTable("AspNetUserLogins");
        }
    }
}