using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Commands;

public record DeleteQuizCommand(Guid Id) : IRequest<ErrorOr<Success>>;

internal class DeleteQuizCommandHandler(IUnitOfWork<Quiz> unitOfWork)
    : IRequestHandler<DeleteQuizCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.Repository.Get(request.Id, cancellationToken);

        if (quiz is null)
        {
            return Error.NotFound("Quiz not found");
        }

        quiz.Delete();

        await unitOfWork.Repository.Remove(quiz, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success;
    }
}
