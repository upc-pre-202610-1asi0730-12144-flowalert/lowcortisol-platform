using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
{
    private readonly List<TEntity> _items;

    protected BaseRepository(List<TEntity> items)
    {
        _items = items;
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _items.Add(entity);
        return Task.CompletedTask;
    }

    public Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_items.FirstOrDefault(item => item.Id == id));
    }

    public Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<TEntity>>(_items.ToList());
    }

    public void Update(TEntity entity)
    {
        var index = _items.FindIndex(item => item.Id == entity.Id);
        if (index >= 0) _items[index] = entity;
    }

    public void Remove(TEntity entity)
    {
        _items.RemoveAll(item => item.Id == entity.Id);
    }
}
