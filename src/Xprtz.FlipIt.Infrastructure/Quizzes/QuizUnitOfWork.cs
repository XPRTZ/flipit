using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Quizzes;

internal class QuizUnitOfWork(FlipItDbContext dbContext)
    : UnitOfWorkBase<Quiz>(dbContext, new QuizRepository(dbContext)),
        IAsyncDisposable;
