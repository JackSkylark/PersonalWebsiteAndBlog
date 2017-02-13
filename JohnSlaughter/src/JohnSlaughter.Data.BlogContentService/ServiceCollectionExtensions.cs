using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using JohnSlaughter.Data.BlogContentService.Store;
using Microsoft.Extensions.Configuration;

namespace JohnSlaughter.Data.BlogContentService
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBlogContentService(
            this IServiceCollection serviceCollection, 
            IConfigurationSection configuration)
        {

            var contentDirectory = configuration["Directory"];

            serviceCollection
                .AddSingleton<IBlogContentStore, BlogMarkdownContentStore>(
                    x => new BlogMarkdownContentStore(contentDirectory));
        }
    }
}
