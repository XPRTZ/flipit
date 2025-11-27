using Microsoft.EntityFrameworkCore;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Infrastructure.Persistence;

internal abstract class UnitOfWorkBase<TEntity>(
    DbContext dbContext,
    IRepository<TEntity> repository
) : IUnitOfWork<TEntity>
    where TEntity : AggregateRoot
{
    public IRepository<TEntity> Repository { get; } = repository;

    public async Task SaveChanges(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    public void Dispose() => dbContext.Dispose();

    public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
}
