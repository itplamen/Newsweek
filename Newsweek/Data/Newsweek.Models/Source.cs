namespace Newsweek.Models
{
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Models.Base;

    public class Source : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string RemoteId { get; set; }
    }
}