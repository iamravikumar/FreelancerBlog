﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFor.Models
{
    public class ArticleTag
    {
        public virtual ICollection<ArticleArticleTag> ArticleArticleTags { get; set; }

        public int ArticleTagId { get; set; }
        public string ArticleTagName { get; set; }

    }
}