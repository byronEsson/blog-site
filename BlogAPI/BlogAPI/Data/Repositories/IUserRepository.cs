using BlogAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq.Expressions;

namespace BlogAPI.Data.Repositories;

public interface IUserRepository<TEntity, T> : IBaseRepository<TEntity, T>
{
    public bool IsEmailUniq(string email);
    public bool IsUsernameUniq(string username);
    public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
}
