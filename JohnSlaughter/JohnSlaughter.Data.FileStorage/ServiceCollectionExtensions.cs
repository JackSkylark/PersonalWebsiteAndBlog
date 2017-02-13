using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using JohnSlaughter.Data.FileStorage.Serializers;
using JohnSlaughter.Data.FileStorage.StoragePathResolvers;
using JohnSlaughter.Data.FileStorage.Models;

namespace JohnSlaughter.Data.FileStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarkdownFileStorage(this IServiceCollection services, StoragePathOptions options)
        {
            services.AddScoped<IStoragePathOptions, StoragePathOptions>(x => options);
            services.AddScoped<IStoragePathResolver, DefaultStoragePathResolver>();
            services.AddScoped<IFileContentSerializer<string>, MarkdownSerializer<string>>();
            services.AddScoped<IFileStorageContext<string>, FileStorageContext<string>>();

            return services;
        }

        public static IServiceCollection AddYamlFileStorage<T>(this IServiceCollection services, StoragePathOptions options) where T : class
        {
            services.AddScoped<IStoragePathOptions, StoragePathOptions>(x => options);
            services.AddScoped<IStoragePathResolver, DefaultStoragePathResolver>();
            services.AddScoped<IFileContentSerializer<T>, YamlSerializer<T>>();
            services.AddScoped<IFileStorageContext<T>, FileStorageContext<T>>();

            return services;
        }

        public static IServiceCollection AddJsonFileStorage<T>(this IServiceCollection services, StoragePathOptions options) where T : class
        {
            services.AddScoped<IStoragePathOptions, StoragePathOptions>(x => options);
            services.AddScoped<IStoragePathResolver, DefaultStoragePathResolver>();
            services.AddScoped<IFileContentSerializer<T>, JsonSerializer<T>>();
            services.AddScoped<IFileStorageContext<T>, FileStorageContext<T>>();

            return services;
        }
    }
}
