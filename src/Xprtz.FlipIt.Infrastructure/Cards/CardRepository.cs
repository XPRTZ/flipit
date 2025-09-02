using Microsoft.EntityFrameworkCore;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Cards;

internal class CardRepository(DbContext dbContext) : RepositoryBase<Card>(dbContext) { }
