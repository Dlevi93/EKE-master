﻿using EKE.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EKE.Data.Entities.Gyopar
{
    public class Article : IEntityBase
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [Required]
        public string Slug { get; set; }
        public int AuthorId { get; set; }
        [Required]
        public Author Author { get; set; }
        public string PublishedBy { get; set; }
        public string Content { get; set; }
        public virtual Magazine Magazine { get; set; }
        public virtual ICollection<ArticleTag> ArticleTag { get; set; }
    }
}
