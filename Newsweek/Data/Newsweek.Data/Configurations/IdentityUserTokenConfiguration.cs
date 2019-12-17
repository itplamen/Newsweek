using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newsweek.Data.Configurations
{
    public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> identityUserToken)
        {
            identityUserToken.Property<string>("UserId");

            identityUserToken.Property<string>("LoginProvider");

            identityUserToken.Property<string>("Name");

            identityUserToken.Property<string>("Value");

            identityUserToken.HasKey("UserId", "LoginProvider", "Name");

            identityUserToken.ToTable("AspNetUserTokens");
        }
    }
}
