namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Source : BaseModel<int>
    {
        public Source()
        {
            News = new HashSet<News>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public ICollection<News> News { get; set; }
    }
}