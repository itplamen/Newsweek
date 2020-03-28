namespace Newsweek.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class GetCommentsListViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NewsId { get; set; }
    }
}