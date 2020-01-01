namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Newsweek.Data.Models;

    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> source)
        {
            source.HasIndex(x => x.Name)
                .IsUnique();

            source.HasMany(x => x.News)
                .WithOne(x => x.Source)
                .HasForeignKey(x => x.SourceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}