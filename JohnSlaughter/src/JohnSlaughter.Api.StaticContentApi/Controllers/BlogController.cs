using JohnSlaughter.Api.StaticContentApi.Model;
using JohnSlaughter.Data.BlogContentService.Store;
using JohnSlaughter.Data.BlogData.Model;
using JohnSlaughter.Data.BlogData.Store;
using JohnSlaughter.Data.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Api.StaticContentApi.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IPostStore _postStore;
        private readonly IBlogContentStore _contentStore;

        public BlogController(IPostStore postStore, IBlogContentStore contentStore)
        {
            _postStore = postStore;
            _contentStore = contentStore;
        }

        [Route("post")]
        [Produces(typeof(Post))]
        [HttpPost]
        public async Task<IActionResult> PublishPost(
            [FromBody] CreatePostModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postStore.CreatePost(model.FileName, model.Title, model.Description, model.PostDate);
            return Ok(post);
        }

        [Route("posts/unpublished")]
        [Produces(typeof(List<string>))]
        [HttpGet]
        public async Task<IActionResult> GetUnpublishedFiles()
        {
            var allFiles = await _contentStore.GetFileNames();
            var allPosts = await _postStore.GetPosts();
            var allPublishedFileNames = allPosts.Select(x => x.FileName);
            var unpublishedFileNames = allFiles.Where(x => !allPublishedFileNames.Contains(x));
            return Ok(unpublishedFileNames);
        } 

        [Route("post/{id}")]
        [Produces(typeof(PostModel))]
        public async Task<IActionResult> GetPost(
            [FromRoute] Guid id)
        {
            try
            {
                var post = await _postStore.GetPost(id);
                var postContent = await _contentStore.GetHtml(post.FileName);
                var postModel = new PostModel {
                    Meta = post,
                    HtmlContent = postContent
                };

                return Ok(postModel);
            } catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }           
        }

        [Route("post")]
        [Produces(typeof(List<Post>))]
        public async Task<IActionResult> FindPost(
            [FromQuery] string title)
        {
            var posts = await _postStore.FindPost(title);
            return Ok(posts);
        }
    }
}
