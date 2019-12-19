namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Data.Models.Base;

    public class News : BaseModel<int>
    {
        public News()
        {
            Caregories = new HashSet<NewsCategory>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string RemoteUrl { get; set; }

        public string RemoteUrlImage { get; set; }

        public string Author { get; set; }

        public int SourceId { get; set; }

        public Source Source { get; set; }

        public ICollection<NewsCategory> Caregories { get; set; }
    }
}