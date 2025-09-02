using Microsoft.EntityFrameworkCore;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Specifications;

namespace Xprtz.FlipIt.Infrastructure.Persistence;

internal abstract class RepositoryBase<TEntity>(DbContext dbContext) : IRepository<TEntity>
    where TEntity : AggregateRoot
{
    protected readonly DbContext DbContext = dbContext;
    protected virtual IQueryable<TEntity> Queryable => DbContext.Set<TEntity>();

    public async Task<TEntity?> Get(Guid id, CancellationToken cancellationToken = default) =>
        await Queryable.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    public async Task<IReadOnlyCollection<TEntity>> GetAll(
        CancellationToken cancellationToken = default
    ) => await Queryable.ToListAsync(cancellationToken: cancellationToken);

    public async Task<IReadOnlyCollection<TEntity>> Find(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) =>
        await Queryable
            .Where(specification.ToExpression())
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<TEntity?> FindOne(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) => await Queryable.FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);

    public async Task Add(TEntity entity, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task AddRange(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    ) => await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

    public Task Remove(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRange(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }
}
