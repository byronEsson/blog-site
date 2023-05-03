using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BlogAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
namespace BlogAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogService<BlogPost, int> _blogService;
        private readonly IBlogService<Comment, int> _commentService;
        public BlogPostsController(IBlogService<BlogPost, int> blogService, IBlogService<Comment, int> commentService)
        {
            _commentService = commentService;
            _blogService = blogService;
        }

        // GET: api/BlogPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            var serviceResponse = await _blogService.GetAllAsync();
            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }
            return serviceResponse.Data.ToList();
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPostId(int id)
        {
            var serviceResponse = await _commentService.GetWhere(c => c.BlogPostId == id);
            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }
            return serviceResponse.Data.ToList();

        }

        // GET: api/BlogPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            var serviceResponse = await _blogService.GetAsync(id);
            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }

            return serviceResponse.Data;
        }

        // PUT: api/BlogPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return BadRequest("Id mismatch.");
            }

            var serviceResponse = await _blogService.UpdateAsync(id, blogPost);

            if (!serviceResponse.WasSuccessful) return NotFound(serviceResponse.Message);
            return NoContent();
        }

        // POST: api/BlogPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost blogPost)
        {
            var serviceResponse = await _blogService.CreateAsync(blogPost);
            if (!serviceResponse.WasSuccessful)
            {
                return Problem(serviceResponse.Message);
            }
            return CreatedAtAction("GetBlogPost", new { id = blogPost.Id }, blogPost);
        }

        [HttpPatch]
        public async Task<ActionResult<BlogPost>> PatchBlogPost(int id, JsonPatchDocument<BlogPost> patch)
        {
            var serviceResponse = await _blogService.UpdateAsync(id, patch);

            if(!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }

            return serviceResponse.Data;
        }

        // DELETE: api/BlogPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var serviceResponse = await _blogService.DeleteAsync(id);

            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }
            return NoContent();
        }
    }
}
