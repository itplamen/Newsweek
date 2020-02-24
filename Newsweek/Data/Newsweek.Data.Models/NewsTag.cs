namespace Newsweek.Data.Models
{
    using System;

    using Newsweek.Data.Models.Contracts;
    
    public class NewsTag : IAuditInfo, IDeletableEntity
    {
        public int NewsId { get; set; }

        public News News { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}