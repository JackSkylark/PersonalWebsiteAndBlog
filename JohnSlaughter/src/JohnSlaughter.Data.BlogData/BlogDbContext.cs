using JohnSlaughter.Data.BlogData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogData
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {

        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options):
            base(options)
        {

        }

        // DbSets
        public DbSet<Post> Post { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlServer(nameof(BlogDbContext));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BlogData.Model.Post.RegisterModel(modelBuilder);
        }
    }
}
