namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    
    public class Email : BaseModel<int>
    {
        public Email()
        {
            Users = new HashSet<UserEmail>();
        }

        public string Subject { get; set; }

        public string Content { get; set; }

        public ICollection<UserEmail> Users { get; set; }
    }
}