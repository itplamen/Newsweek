namespace Newsweek.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Common.Validation;
    using Newsweek.Handlers.Commands.Comments;
    
    public class CreateCommentViewModel : IMapTo<CreateCommentCommand>
    {
        [Required]
        public int NewsId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ValidationConstants.COMMENT_MAX_CONTENT_LENGTH)]
        public string Content { get; set; }
    }
}