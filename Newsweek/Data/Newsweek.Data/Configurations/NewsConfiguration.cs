namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newsweek.Models;

    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> news)
        {
            news.HasOne(x => x.Source)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.SourceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}