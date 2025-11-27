using Xprtz.FlipIt.Domain.SeedWork.Specifications;

namespace Xprtz.FlipIt.Domain.SeedWork;

public interface IRepository<TEntity>
    where TEntity : AggregateRoot
{
    Task<TEntity?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> Find(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default
    );
    Task<TEntity?> FindOne(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default
    );
    Task Add(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task Remove(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
