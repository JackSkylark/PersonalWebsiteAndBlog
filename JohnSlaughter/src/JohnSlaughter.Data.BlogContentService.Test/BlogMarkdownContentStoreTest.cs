using JohnSlaughter.Data.BlogContentService.Store;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogContentService.Test
{
    [TestFixture(Category = "ApiTest-Functional")]
    public class BlogMarkdownContentStoreTest
    {
        private BlogMarkdownContentStore _store;

        public BlogMarkdownContentStoreTest()
        {
            _store = new BlogMarkdownContentStore(@"G:\Development\Markdown");
        }

        [Test]
        public async Task CanReadMarkdown()
        {
            var content = await _store.GetHtml("test.md");
            Assert.IsNotEmpty(content);
        }

    }
}
