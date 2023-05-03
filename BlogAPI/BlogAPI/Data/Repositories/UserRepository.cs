using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogAPI.Data.Repositories;

public class UserRepository : BaseRepository<User, string> , IUserRepository<User, string>
{
    public UserRepository (BlogContext context) : base(context)
    {
    }

    public bool IsEmailUniq(string email)
    {
        var user = GetSingle(u => u.Email == email);
        return user == null;
    }

    public bool IsUsernameUniq(string username)
    {
        var user = GetSingle(u => u.UserName == username);
        return user == null;
    }
}
