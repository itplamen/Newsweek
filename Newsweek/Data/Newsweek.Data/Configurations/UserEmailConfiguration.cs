namespace Newsweek.Data.Configurations
{
    using Newsweek.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    public class UserEmailConfiguration : IEntityTypeConfiguration<UserEmail>
    {
        public void Configure(EntityTypeBuilder<UserEmail> userEmail)
        {
            userEmail.HasKey(x => new { x.UserId, x.EmailId });

            userEmail.HasOne(x => x.User)
                .WithMany(x => x.Emails)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            userEmail.HasOne(x => x.Email)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.EmailId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}