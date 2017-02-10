using JohnSlaughter.Data.BlogData.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogData
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBlogData(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction)
        {
            serviceCollection
                .AddDbContext<BlogDbContext>(optionsAction, ServiceLifetime.Transient)
                .AddTransient<DbContext, BlogDbContext>()
                .AddTransient<Func<BlogDbContext>>(
                    provider => provider.GetRequiredService<BlogDbContext>);

            serviceCollection
                .AddSingleton<IPostStore, PostStore>();
        }
    }
}
