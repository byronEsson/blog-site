using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace BlogAPI.Data.Repositories
{
    public class PostRepository : BaseRepository<BlogPost, int> 
    {
        public PostRepository(BlogContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Comments).ToListAsync();
        }
        public async override Task<BlogPost> FindAsync(int id)
        {
            return await _dbSet.Where(p => p.Id == id).Include("Comments").FirstOrDefaultAsync();
        }
    }
}
