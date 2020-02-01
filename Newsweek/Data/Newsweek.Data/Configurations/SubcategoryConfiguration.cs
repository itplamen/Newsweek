namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newsweek.Data.Models;

    public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> subcategory)
        {
            subcategory.HasIndex(x => x.Name)
                .IsUnique();

            subcategory.HasOne(x => x.Category)
                .WithMany(x => x.Subcategories)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            subcategory.HasMany(x => x.News)
                .WithOne(x => x.Subcategory)
                .HasForeignKey(x => x.SubcategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}