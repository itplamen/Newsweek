namespace Newsweek.Data.Configurations
{
    using Newsweek.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> email)
        {
            email.Property(x => x.Subject)
                .IsRequired();

            email.Property(x => x.Content)
                .IsRequired();
        }
    }
}