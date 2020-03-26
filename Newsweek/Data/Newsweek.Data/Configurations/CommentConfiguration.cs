namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newsweek.Common.Validation;
    using Newsweek.Data.Models;

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> comment)
        {
            comment.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(ValidationConstants.COMMENT_MAX_CONTENT_LENGTH);

            comment.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            comment.HasOne(x => x.News)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.NewsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
