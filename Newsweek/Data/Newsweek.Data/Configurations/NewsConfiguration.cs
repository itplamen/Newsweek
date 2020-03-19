namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newsweek.Data.Models;

    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> news)
        {
            news.Property(x => x.Title)
                .IsRequired();

            news.Property(x => x.Content)
                .IsRequired();

            news.Property(x => x.Description)
                .IsRequired();

            news.Property(x => x.RemoteUrl)
                .IsRequired();

            news.HasIndex(x => x.RemoteUrl)
                .IsUnique();

            news.HasOne(x => x.Source)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.SourceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            news.HasOne(x => x.Subcategory)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.SubcategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            news.HasMany(x => x.Comments)
                .WithOne(x => x.News)
                .HasForeignKey(x => x.NewsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}