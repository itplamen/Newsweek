namespace Newsweek.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Commands.Comments;
    
    public class CreateCommentViewModel : IMapTo<CreateCommentCommand>
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int NewsId { get; set; }
    }
}