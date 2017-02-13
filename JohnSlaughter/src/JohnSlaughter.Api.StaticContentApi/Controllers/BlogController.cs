using JohnSlaughter.Api.StaticContentApi.Model;
using JohnSlaughter.Data.FileStorage;
using JohnSlaughter.Data.FileStorage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Api.StaticContentApi.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IFileStorageContext<string> _markdownStorageContext;
        private readonly IFileStorageContext<PostMeta> _yamlStorageContext;
             
        public BlogController(IFileStorageContext<string> markdownStorageContext, IFileStorageContext<PostMeta> yamlStorageContext)
        {
            _markdownStorageContext = markdownStorageContext;
            _yamlStorageContext = yamlStorageContext;
        }

        [Route("posts/meta")]
        [Produces(typeof(Dictionary<string, PostMeta>))]
        [HttpGet]
        public async Task<IActionResult> GetAllMeta()
        {
            var posts = await _yamlStorageContext.GetAll();
            return Ok(posts);
        }

        [Route("posts/meta/{key}")]
        [Produces(typeof(Dictionary<string, PostMeta>))]
        [HttpGet]
        public async Task<IActionResult> GetMeta([FromRoute] string key)
        {
            var post = await _yamlStorageContext.Get(key);
            return Ok(post);
        }

        [Route("posts/meta/paged")]
        [Produces(typeof(Dictionary<string, PostMeta>))]
        [HttpGet]
        public async Task<IActionResult> GetPagedMeta([FromQuery] int page, [FromQuery] int pageSize = 10)
        {
            var posts = await _yamlStorageContext.GetAll();
            var sortedPosts = posts
                .OrderByDescending(x => x.Value.CreateDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToDictionary(x => x.Key, x => x.Value);
            return Ok(sortedPosts);
        }

        [Route("posts/{key}")]
        [Produces(typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetPost([FromRoute] string key)
        {
            var post = await _markdownStorageContext.Get(key);

            if(!post.ContainsKey(key))
            {
                return NotFound(key);
            }

            return Ok(post[key]);
        }

        //[Route("posts")]
        //[Produces(typeof(List<string>))]
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var posts = await _storageContext.GetAll("posts");
        //    return Ok(posts);
        //}

        //[Route("post/count")]
        //[Produces(typeof(int))]
        //[HttpGet]
        //public async Task<IActionResult> GetCount()
        //{
        //    var count = await _storageContext.GetCount("posts");
        //    return Ok(count);
        //}

        //[Route("post")]
        //[Produces(typeof(string))]
        //[HttpPost]
        //public async Task<IActionResult> PublishPost(
        //    [FromBody] CreatePostModel model)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var post = await _postStore.CreatePost(model.FileName, model.Title, model.Description, model.PostDate);
        //    return Ok(post);
        //}

        //[Route("posts/unpublished")]
        //[Produces(typeof(List<string>))]
        //[HttpGet]
        //public async Task<IActionResult> GetUnpublishedFiles()
        //{
        //    var allFiles = await _contentStore.GetFileNames();
        //    var allPosts = await _postStore.GetPosts();
        //    var allPublishedFileNames = allPosts.Select(x => x.FileName);
        //    var unpublishedFileNames = allFiles.Where(x => !allPublishedFileNames.Contains(x));
        //    return Ok(unpublishedFileNames);
        //} 

        //[Route("post/{id}")]
        //[Produces(typeof(PostModel))]
        //public async Task<IActionResult> GetPost(
        //    [FromRoute] Guid id)
        //{
        //    try
        //    {
        //        var post = await _postStore.GetPost(id);
        //        var postContent = await _contentStore.GetHtml(post.FileName);
        //        var postModel = new PostModel {
        //            Meta = post,
        //            HtmlContent = postContent
        //        };

        //        return Ok(postModel);
        //    } catch (EntityNotFoundException e)
        //    {
        //        return NotFound(e.Message);
        //    }           
        //}

        //[Route("post")]
        //[Produces(typeof(List<Post>))]
        //public async Task<IActionResult> FindPost(
        //    [FromQuery] string title)
        //{
        //    var posts = await _postStore.FindPost(title);
        //    return Ok(posts);
        //}
    }
}
