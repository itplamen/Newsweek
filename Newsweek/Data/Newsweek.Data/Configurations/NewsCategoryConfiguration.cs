namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Newsweek.Data.Models;

    public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
    {
        public void Configure(EntityTypeBuilder<NewsCategory> newsCategory)
        {
            newsCategory.HasKey(x => new { x.NewsId, x.CategoryId });

            newsCategory.HasOne(x => x.News)
                .WithMany(x => x.Caregories)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Restrict);

            newsCategory.HasOne(x => x.Category)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}