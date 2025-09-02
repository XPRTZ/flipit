using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Topics;

internal class TopicUnitOfWork(FlipItDbContext dbContext)
    : UnitOfWorkBase<Topic>(dbContext, new TopicRepository(dbContext)),
        IAsyncDisposable;
