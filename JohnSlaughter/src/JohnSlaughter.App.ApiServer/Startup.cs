using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using JohnSlaughter.Api.StaticContentApi;
using JohnSlaughter.Data.BlogData;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.Entity.Extensions;
using JohnSlaughter.Data.BlogContentService;

namespace JohnSlaughter.App.ApiServer
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Db Contexts
            services.AddBlogData(options => options.UseMySQL(
                _configuration.GetConnectionString("BlogDb")));


            services.AddBlogContentService(
                _configuration.GetSection("BlogContent"));

            // MVC & API Controllers.
            services.AddMvc(SetupMvc)
                .AddBlogStaticContentApi();

            services.AddMvcCore()
                .AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime,
            IServiceProvider serviceProvider)
        {
            foreach (var dbContext in serviceProvider.GetServices<DbContext>())
            {
                dbContext.Database.Migrate();
            }

            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void SetupMvc(MvcOptions options)
        {
            //options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider())
        }
    }
}
