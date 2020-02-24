namespace Newsweek.Data.Models
{
    using System;

    using Newsweek.Data.Models.Contracts;

    public class UserEmail : IAuditInfo, IDeletableEntity
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int EmailId { get; set; }

        public Email Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}