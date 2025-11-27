using Xprtz.FlipIt.Application.Quizzes.Services;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Quizzes.Commands;

public record CreateQuizCommand(
    string TopicName,
    int? NumberOfQuestions,
    FrontOrBack ShowFirst,
    bool IsPracticeMode,
    IReadOnlyCollection<Card> Cards
) : IRequest<ErrorOr<Quiz>>;

internal class CreateQuizCommandHandler(IUnitOfWork<Quiz> unitOfWork, IQuestionsForNextQuizService questionsForNextQuizService)
    : IRequestHandler<CreateQuizCommand, ErrorOr<Quiz>>
{
    public async Task<ErrorOr<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var (topicName, numberOfQuestions, showFirst, isPracticeMode, cards) = request;

        var questions = questionsForNextQuizService.CreateQuestions(cards, numberOfQuestions);

        var result = Quiz.Create(topicName, showFirst, isPracticeMode, questions);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.Repository.Add(result.Value, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}
