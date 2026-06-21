using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Shared.Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
