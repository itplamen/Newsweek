namespace Newsweek.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Common.Validation;
    using Newsweek.Data.Models;

    public class UpdateCommentViewModel : IMapTo<Comment>
    {
        [Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ValidationConstants.COMMENT_MAX_CONTENT_LENGTH)]
        public string Content { get; set; }
    }
}