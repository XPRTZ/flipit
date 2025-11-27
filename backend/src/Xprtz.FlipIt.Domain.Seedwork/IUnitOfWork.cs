namespace Xprtz.FlipIt.Domain.SeedWork;

public interface IUnitOfWork<TEntity> : IDisposable where TEntity : AggregateRoot
{
    IRepository<TEntity> Repository { get; }
    
    Task SaveChanges(CancellationToken cancellationToken = default);
}
