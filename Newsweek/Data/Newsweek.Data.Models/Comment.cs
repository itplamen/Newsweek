namespace Newsweek.Data.Models
{
    public class Comment : BaseModel<int>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int NewsId { get; set; }

        public News News { get; set; }
    }
}