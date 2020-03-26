namespace Newsweek.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class UpdateCommentViewModel : IMapTo<Comment>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}