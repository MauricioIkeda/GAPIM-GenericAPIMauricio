using AutoMapper;
using GAPIM_GenericAPIMauricio.Entities;
using GAPIM_GenericAPIMauricio.Interfaces;

namespace GAPIM_GenericAPIMauricio.Services;

public class GenericService<TEntity, TRequest, TResponse> : IGenericService<TEntity, TRequest, TResponse> where TEntity : BaseEntity
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    
    public GenericService(IGenericRepository<TEntity> repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<TResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException("Entidade nao encontrada");
        }
        
        return _mapper.Map<TResponse>(entity);
    }

    public async Task<List<TResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<List<TResponse>>(entities);
    }

    public async Task<TResponse> AddAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(request);
        
        var createdEntity = await _repository.AddAsync(entity, cancellationToken);
        
        return _mapper.Map<TResponse>(createdEntity);
    }

    public async Task<TResponse> UpdateAsync(Guid id, TRequest request, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _repository.GetByIdAsync(id, cancellationToken);

        if (entityToUpdate == null)
        {
            throw new KeyNotFoundException("Entidade não encontrada.");
        }

        _mapper.Map(request, entityToUpdate); 
    
        var updatedEntity = await _repository.UpdateAsync(id, entityToUpdate, cancellationToken);
    
        return _mapper.Map<TResponse>(updatedEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var success = await _repository.DeleteAsync(id, cancellationToken);
        
        return success;
    }
}