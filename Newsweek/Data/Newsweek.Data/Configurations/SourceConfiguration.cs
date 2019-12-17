namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Newsweek.Models;

    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> source)
        {
            source.HasMany(x => x.News)
                .WithOne(x => x.Source)
                .HasForeignKey(x => x.SourceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}