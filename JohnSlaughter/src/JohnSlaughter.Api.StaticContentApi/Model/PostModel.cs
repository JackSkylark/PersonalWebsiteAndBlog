using JohnSlaughter.Data.BlogData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Api.StaticContentApi.Model
{
    public class PostModel
    {
        public string HtmlContent { get; set; }
        public Post Meta { get; set; }
    }
}
