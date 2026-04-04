using GAPIM_GenericAPIMauricio.Entities;

namespace GAPIM_GenericAPIMauricio.Interfaces;

public interface IGenericService<TEntity, TRequest, TResponse> where TEntity : BaseEntity
{
    public Task<TResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<TResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TResponse> AddAsync(TRequest request, CancellationToken cancellationToken = default);
    public Task<TResponse> UpdateAsync(Guid id, TRequest entity, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}