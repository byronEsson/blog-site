using System.Linq.Expressions;

namespace BlogAPI.Data.Repositories;

public interface IBaseRepository<T, TId>
{
    bool IsNull { get; }
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> FindAsync(TId id);

    T? GetSingle (Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);

    Task<IEnumerable<T?>> FindWhere(Expression<Func<T, bool>> predicate);
    void Update(T entity);
    void Remove(T entity);
    Task SaveAsync();
}

