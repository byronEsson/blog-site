using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlogAPI.Data.Repositories;

public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
{
    private readonly BlogContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(BlogContext context)
    {
        _dbSet = context.Set<TEntity>();
        _context = context;
    }

    public bool IsNull => _dbSet == null;

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }


    public virtual async Task<TEntity?> FindAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task<IEnumerable<TEntity?>> FindWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
    public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
    {
        return  _context.Set<TEntity>().FirstOrDefault(predicate);
    }
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public async Task UpdateSimple(TEntity entity, JsonPatchDocument<TEntity> document)
    {
        document.ApplyTo(entity);

        _context.Update(entity);
        _context.SaveChanges();
    }
}
