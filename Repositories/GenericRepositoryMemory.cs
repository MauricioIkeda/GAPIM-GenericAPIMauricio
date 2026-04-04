using System.Collections.Concurrent;
using GAPIM_GenericAPIMauricio.DataBase;
using GAPIM_GenericAPIMauricio.Entities;
using GAPIM_GenericAPIMauricio.Interfaces;

namespace GAPIM_GenericAPIMauricio.Repositories;

public class GenericRepositoryMemory<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ConcurrentDictionary<Guid, T> _dbSet;

    public GenericRepositoryMemory(MemoryContext context)
    {
        _dbSet = context.Set<T>();
    }
    
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _dbSet.TryGetValue(id, out T entity);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var entities =  _dbSet.Values.AsEnumerable();
        return Task.FromResult(entities);
    }

    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        
        entity.CreatedAt = DateTime.UtcNow;

        if (!_dbSet.TryAdd(entity.Id, entity))
        {
            throw new Exception("Nao foi possivel salvar a entidade");
        }
        
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(Guid id, T entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (!_dbSet.ContainsKey(id))
        {
            throw new KeyNotFoundException("Entidade não encontrada.");
        }

        entity.UpdatedAt = DateTime.UtcNow;
        entity.Id = id;
    
        _dbSet[id] = entity; 

        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if(!_dbSet.TryRemove(id, out _))
        {
            throw new Exception("Nao foi possivel remover esta entidade");
        }
        
        return Task.FromResult(true);
    }
}