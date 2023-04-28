using BlogAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogAPI.Services;

public class BlogService<TEntity, TId> : IBlogService<TEntity, TId> where TEntity : class
{
    internal IBaseRepository<TEntity, TId> _repository;
    

    public BlogService(IBaseRepository<TEntity, TId> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<TEntity>> CreateAsync(TEntity entity)
    {
        var response = new ServiceResponse<TEntity>();
        if (_repository.IsNull)
        {
            SetRepositoryNullResponse(ref response);
            return response;
        }
        else
        {
            _repository.Add(entity);
            await _repository.SaveAsync();
            response.Data = entity;
            return response;
        }
    }

    public async Task<ServiceResponse<TEntity>> DeleteAsync(TId id)
    {
        var response = new ServiceResponse<TEntity>();
        if (_repository.IsNull)
        {
            SetRepositoryNullResponse(ref response);
            return response;
        }

        var entity = await _repository.FindAsync(id);

        if (entity == null)
        {
            SetEntityNullResponse(ref response, id);
            return response;
        }

        _repository.Remove(entity);

        await _repository.SaveAsync();

        return response;
    }

    public async Task<ServiceResponse<IEnumerable<TEntity>?>> GetAllAsync()
    {
        var response = new ServiceResponse<IEnumerable<TEntity>?>();

        if (_repository.IsNull)
        {
            SetRepositoryNullResponse(ref response);
            return response;
        }
        response.Data = await _repository.GetAllAsync();
        
        return response;
    }

    public async Task<ServiceResponse<TEntity?>> GetAsync(TId id)
    {
        var response = new ServiceResponse<TEntity?>();
        if (_repository.IsNull)
        {
            SetRepositoryNullResponse(ref response);
            return response;
        }

        TEntity entity = await _repository.FindAsync(id);

        if (entity == null)
        {
            SetEntityNullResponse(ref response, id);
            return response;
        }
        response.Data = entity;
        return response;
    }

    public async Task SaveAsync()
    {
        await _repository.SaveAsync();
    }

    public async Task<ServiceResponse<TEntity>> UpdateAsync(TId id, TEntity entity)
    {
        var response = new ServiceResponse<TEntity>();
        response.Data = entity;
        _repository.Update(entity);
        try
        {
            await _repository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EntityExists(id))
            {
                SetEntityNullResponse(ref response, id);
                return response;
            }

            else throw;
        }
        return response;
    }

    private async Task<bool> EntityExists(TId id)
    {
        return await _repository.FindAsync(id) != null;
    }

    private void SetRepositoryNullResponse<TData>(ref ServiceResponse<TData> response)
    {
        response.WasSuccessful = false;
        response.Message = $"Entity set '{typeof(TEntity).Name}' is null.";
    }
    private void SetEntityNullResponse<TData>(ref ServiceResponse<TData> response, TId id)
    {
        response.WasSuccessful = false;
        response.Message = $"Could not find entity with id: {id}.";
    }

    public ServiceResponse<TEntity> FindSingle(Expression<Func<TEntity, bool>> predicate)
    {
        var response = new ServiceResponse<TEntity>();
        if (_repository.IsNull)
        {
            SetRepositoryNullResponse(ref response);
            return response;
        }

        var entity = _repository.GetSingle(predicate);

        if (entity == null)
        {
            response.Message = "No user with this email";
            response.WasSuccessful = false;
            return response;
        }

        response.Data = entity;

        return response;
    }
}
