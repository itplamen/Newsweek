namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Data.Models.Base;

    public class Source : BaseModel<int>
    {
        public Source()
        {
            News = new HashSet<News>();
        }

        [Required]
        public string Name { get; set; }

        public string RemoteId { get; set; }

        public ICollection<News> News { get; set; }
    }
}