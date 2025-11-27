using Microsoft.EntityFrameworkCore;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Quizzes;

internal class QuizRepository(DbContext dbContext) : RepositoryBase<Quiz>(dbContext)
{
    protected override IQueryable<Quiz> Queryable =>
        DbContext.Set<Quiz>().Include(x => x.Questions);
}
