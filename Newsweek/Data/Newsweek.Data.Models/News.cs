namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    public class News : BaseModel<int>
    {
        public News()
        {
            Tags = new HashSet<NewsTag>();
            Comments = new HashSet<Comment>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public int SourceId { get; set; }

        public Source Source { get; set; }

        public int SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        public ICollection<NewsTag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}