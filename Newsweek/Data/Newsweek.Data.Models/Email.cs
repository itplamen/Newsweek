namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Email : BaseModel<int>
    {
        public Email()
        {
            Users = new HashSet<UserEmail>();
        }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ICollection<UserEmail> Users { get; set; }
    }
}