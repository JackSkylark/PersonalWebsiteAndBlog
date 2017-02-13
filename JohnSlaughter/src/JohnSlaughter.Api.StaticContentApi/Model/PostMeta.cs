using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Api.StaticContentApi.Model
{
    public class PostMeta
    {
        public PostMeta()
        {
            CreateDate = DateTime.MinValue;
        }

        public PostMeta(string author, string title, string description)
        {
            Author = author;
            Title = title;
            Description = description;
        }

        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
