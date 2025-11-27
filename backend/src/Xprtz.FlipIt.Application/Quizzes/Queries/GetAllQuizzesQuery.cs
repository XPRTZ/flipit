using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Queries;

public record GetAllQuizzesQuery() : IRequest<IReadOnlyCollection<Quiz>>;

internal class GetAllQuizzesQueryHandler(IUnitOfWork<Quiz> unitOfWork)
    : IRequestHandler<GetAllQuizzesQuery, IReadOnlyCollection<Quiz>>
{
    public async Task<IReadOnlyCollection<Quiz>> Handle(
        GetAllQuizzesQuery request,
        CancellationToken cancellationToken
    ) => await unitOfWork.Repository.GetAll(cancellationToken);
}
