using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Cards;

internal class CardUnitOfWork(FlipItDbContext dbContext)
    : UnitOfWorkBase<Card>(dbContext, new CardRepository(dbContext)),
        IAsyncDisposable;
