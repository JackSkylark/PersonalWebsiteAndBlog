using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage.Models
{
    public class MarkdownFileContent<TMeta>
    {
        public string Key { get; set; }
        public TMeta Meta { get; set; }
        public string Html { get; set; }
    }
}
