namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Newsweek.Data.Models;

    public class NewsTagConfiguration : IEntityTypeConfiguration<NewsTag>
    {
        public void Configure(EntityTypeBuilder<NewsTag> newsTag)
        {
            newsTag.HasKey(x => new { x.NewsId, x.TagId });

            newsTag.HasOne(x => x.News)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Restrict);

            newsTag.HasOne(x => x.Tag)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}