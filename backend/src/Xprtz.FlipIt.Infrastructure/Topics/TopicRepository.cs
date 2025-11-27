using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Topics;

internal class TopicRepository(FlipItDbContext dbContext)
    : RepositoryBase<Topic>(dbContext) { }
