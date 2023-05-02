using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.JsonPatch;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IBlogService<Comment, int> _blogService;

        public CommentsController(IBlogService<Comment, int> blogService)
        {
            _blogService = blogService;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var serviceResponse = await _blogService.GetAllAsync();
            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }
            return serviceResponse.Data.ToList();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var serviceResponse = await _blogService.GetAsync(id);
            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }

            return serviceResponse.Data;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Comment>> PatchComment(int id, JsonPatchDocument<Comment> patch)
        {
            var serviceResponse = await _blogService.UpdateAsync(id, patch);

            if (!serviceResponse.WasSuccessful)
            {
                return NotFound(serviceResponse.Message);
            }

            return serviceResponse.Data;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("Id mismatch.");
            }

            var serviceResponse = await _blogService.UpdateAsync(id, comment);

            if (!serviceResponse.WasSuccessful) return NotFound(serviceResponse.Message);
            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            var serviceResponse = await _blogService.CreateAsync(comment);
            if (!serviceResponse.WasSuccessful)
            {
                return Problem(serviceResponse.Message);
            }
            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
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
