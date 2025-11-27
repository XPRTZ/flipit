using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Commands;

public record AnswerQuestionCommand(Guid QuizId, Guid QuestionId, bool IsCorrect) : IRequest<ErrorOr<Success>>;

internal class AnswerQuestionCommandHandler(IUnitOfWork<Quiz> unitOfWork)
    : IRequestHandler<AnswerQuestionCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.Repository.Get(request.QuizId, cancellationToken);

        if (quiz is null)
        {
            return Error.NotFound("Quiz not found");
        }

        var result = quiz.AnswerQuestion(request.QuestionId, request.IsCorrect);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success;
    }
}
