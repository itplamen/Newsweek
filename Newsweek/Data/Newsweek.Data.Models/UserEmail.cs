namespace Newsweek.Data.Models
{
    public class UserEmail : BaseModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int EmailId { get; set; }

        public Email Email { get; set; }
    }
}