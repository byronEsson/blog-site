using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace BlogAPI.Services;

public interface IBlogService<TEntity, TId> where TEntity : class
{
    Task<ServiceResponse<TEntity>> CreateAsync(TEntity entity);
    Task<ServiceResponse<TEntity>> UpdateAsync(TId id, TEntity entity);
    Task<ServiceResponse<TEntity>> UpdateAsync(TId id, JsonPatchDocument<TEntity> patch);
    Task<ServiceResponse<TEntity>> DeleteAsync(TId id);
    Task<ServiceResponse<IEnumerable<TEntity>?>> GetAllAsync();
    Task<ServiceResponse<IEnumerable<TEntity>?>> GetWhere(Expression<Func<TEntity, bool>> predicate);
    Task<ServiceResponse<TEntity?>> GetAsync(TId id);
    ServiceResponse<TEntity> FindSingle(Expression<Func<TEntity, bool>> predicate);
    Task SaveAsync();
}
