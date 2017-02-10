using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JohnSlaughter.Api.StaticContentApi
{
    public static class StaticContentApiMvcBuilderExtensions
    {
        public static IMvcBuilder AddBlogStaticContentApi(this IMvcBuilder app)
        {
            var assembly = typeof(StaticContentApiMvcBuilderExtensions).GetTypeInfo().Assembly;
            return app.AddApplicationPart(assembly);
        }
    }
}
