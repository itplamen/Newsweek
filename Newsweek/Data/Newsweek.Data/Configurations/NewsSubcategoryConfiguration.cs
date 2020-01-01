namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Newsweek.Data.Models;

    public class NewsSubcategoryConfiguration : IEntityTypeConfiguration<NewsSubcategory>
    {
        public void Configure(EntityTypeBuilder<NewsSubcategory> newsSubcategory)
        {
            newsSubcategory.HasKey(x => new { x.NewsId, x.SubcategoryId });

            newsSubcategory.HasOne(x => x.News)
                .WithMany(x => x.Subcategories)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Restrict);

            newsSubcategory.HasOne(x => x.Subcategory)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}