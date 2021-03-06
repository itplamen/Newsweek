﻿namespace Newsweek.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Data.Models.Contracts;

    public abstract class BaseModel<TKey> : IAuditInfo, IDeletableEntity
    {
        [Key]
        public TKey Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}