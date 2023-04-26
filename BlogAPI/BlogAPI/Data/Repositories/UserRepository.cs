using BlogAPI.Models;

namespace BlogAPI.Data.Repositories;

public class UserRepository<T, TId> : BaseRepository<T, TId>, IUserRepository<T, TId> where T : User
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
