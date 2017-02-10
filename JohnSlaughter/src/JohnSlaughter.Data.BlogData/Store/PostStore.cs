using JohnSlaughter.Data.BlogData.Model;
using JohnSlaughter.Data.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogData.Store
{
    public class PostStore : IPostStore
    {
        private readonly Func<BlogDbContext> _createBlogDbContext;

        public PostStore(Func<BlogDbContext> createBlogDbContext)
        {
            _createBlogDbContext = createBlogDbContext;
        }

        public async Task<Post> CreatePost(
            string fileName,
            string title, 
            string description, 
            DateTime postDate)
        {

            using (var db = _createBlogDbContext())
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    PostDate = postDate,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    FileName = fileName
                };

                db.Post.Add(post);

                await db.SaveChangesAsync();

                return post;
            }
        }

        public async Task<List<Post>> GetPosts()
        {
            using (var db = _createBlogDbContext())
            {
                return await db.Post.ToListAsync();
            }
        }

        public async Task<Post> GetPost(Guid id)
        {
            using (var db = _createBlogDbContext())
            {
                return await GetPost(db, id);
            }
        }

        public async Task<List<Post>> FindPost(string title)
        {
            using (var db = _createBlogDbContext())
            {
                return await db.Post.Where(x => x.Title.Contains(title)).ToListAsync();
            }
        }

        public async Task<Post> UpdatePost(
            Guid id,
            string title,
            string description,
            DateTime postDate)
        {
            using (var db = _createBlogDbContext())
            {
                var post = await GetPost(db, id);
                post.Title = title;
                post.Description = description;
                post.PostDate = postDate;

                await db.SaveChangesAsync();

                return post;
            }
        }

        public async Task<Post> UpdatePost(
            string fileName,
            string title,
            string description,
            DateTime postDate)
        {
            using (var db = _createBlogDbContext())
            {
                var post = await GetPost(db, fileName);
                post.Title = title;
                post.Description = description;
                post.PostDate = postDate;

                await db.SaveChangesAsync();

                return post;
            }
        }


        private static async Task<Post> GetPost(BlogDbContext db, Guid id)
        {
            var post = await db.Post
                    .Where(x => x.Id == id)
                    .SingleOrDefaultAsync();

            if (post == null)
            {
                throw new EntityNotFoundException($"Post not found with Id = '{id}'");
            }

            return post;
        }

        private static async Task<Post> GetPost(BlogDbContext db, string fileName)
        {
            var post = await db.Post
                .Where(x => x.FileName == fileName)
                .SingleOrDefaultAsync();

            if(post == null)
            {
                throw new EntityNotFoundException($"Post not found with FileName = '{fileName}'");
            }

            return post;
        }
    }
}
