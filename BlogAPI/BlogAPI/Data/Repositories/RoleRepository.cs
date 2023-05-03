using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Data.Repositories
{
    public class RoleRepository : BaseRepository<IdentityUserRole<string>, string>
    {
        public RoleRepository(BlogContext context) : base(context)
        {
        }

       
    }
}
