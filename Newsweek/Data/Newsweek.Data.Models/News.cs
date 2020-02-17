﻿namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class News : BaseModel<int>
    {
        public News()
        {
            Tags = new HashSet<NewsTag>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public int SourceId { get; set; }

        public Source Source { get; set; }

        public int SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        public ICollection<NewsTag> Tags { get; set; }
    }
}