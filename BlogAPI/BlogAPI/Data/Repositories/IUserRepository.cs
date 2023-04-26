using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BlogAPI.Data.Repositories;

public interface IUserRepository<TEntity, T> : IBaseRepository<TEntity, T>
{
    public bool IsEmailUniq(string email);
    public bool IsUsernameUniq(string username);
}
