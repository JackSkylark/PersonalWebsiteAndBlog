using JohnSlaughter.Data.BlogData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogData.Store
{
    public interface IPostStore
    {
        Task<Post> CreatePost(string fileName, string title, string description, DateTime postDate);
        Task<Post> GetPost(Guid id);
        Task<List<Post>> GetPosts();
        Task<List<Post>> FindPost(string title);
        Task<Post> UpdatePost(Guid id, string title, string description, DateTime postDate);
        Task<Post> UpdatePost(string fileName, string title, string description, DateTime postDate);
    }
}
