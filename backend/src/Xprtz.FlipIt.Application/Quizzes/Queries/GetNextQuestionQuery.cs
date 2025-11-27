using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Queries;

public record GetNextQuestionQuery(Guid QuizId) : IRequest<ErrorOr<Question>>;

internal class GetNextQuestionQueryHandler(IUnitOfWork<Quiz> unitOfWork)
    : IRequestHandler<GetNextQuestionQuery, ErrorOr<Question>>
{
    public async Task<ErrorOr<Question>> Handle(GetNextQuestionQuery request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.Repository.Get(request.QuizId, cancellationToken);

        return quiz?.GetNextQuestion() ?? Error.NotFound("Quiz not found");
    }
}
