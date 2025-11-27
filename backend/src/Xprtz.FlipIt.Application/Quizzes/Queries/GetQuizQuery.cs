using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Queries;

public record GetQuizQuery(Guid Id) : IRequest<ErrorOr<Quiz>>;

internal class GetQuizQueryHandler(IUnitOfWork<Quiz> unitOfWork) : IRequestHandler<GetQuizQuery, ErrorOr<Quiz>>
{
    public async Task<ErrorOr<Quiz>> Handle(GetQuizQuery request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.Repository.Get(request.Id, cancellationToken);

        if (quiz is null)
        {
            return Error.NotFound("Card not found");
        }

        return quiz;
    }
}
