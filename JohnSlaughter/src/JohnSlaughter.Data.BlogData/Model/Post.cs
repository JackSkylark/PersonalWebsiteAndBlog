using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogData.Model
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
        public static void RegisterModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Post>();

            entity
                .HasKey(x => x.Id);

            entity
                .Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            entity
                .Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity
                .Property(x => x.FileName)
                .IsRequired();

            entity
                .Property(x => x.PostDate)
                .IsRequired();

            entity
                .Property(x => x.CreatedDate)
                .IsRequired();

            entity
                .Property(x => x.LastModifiedDate)
                .IsRequired();

            entity
                .HasIndex(x => new
                {
                    x.FileName
                }).IsUnique();

        }
    }
}
