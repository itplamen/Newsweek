namespace Newsweek.Web.Models
{
    using System;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    
    public abstract class BaseViewModel : IMapFrom<BaseModel<int>>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}