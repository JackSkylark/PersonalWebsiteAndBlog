using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogContentService.Model
{
    public class Post
    {
        public PostMeta Meta { get; set; }
        public string HtmlContent { get; set; }
    }
}
