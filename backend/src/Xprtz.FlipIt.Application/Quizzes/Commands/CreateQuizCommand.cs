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

internal class CreateQuizCommandHandler(IUnitOfWork<Quiz> unitOfWork, IEnumerable<IIncludeInQuizRule> rules)
    : IRequestHandler<CreateQuizCommand, ErrorOr<Quiz>>
{
    public async Task<ErrorOr<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var (topicName, numberOfQuestions, showFirst, isPracticeMode, cards) = request;

        var questions = CreateQuestions(cards, numberOfQuestions);

        var result = Quiz.Create(topicName, showFirst, isPracticeMode, questions);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.Repository.Add(result.Value, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return result;
    }

    private IReadOnlyCollection<Question> CreateQuestions(IReadOnlyCollection<Card> cards, int? numberOfQuestions) =>
        cards
            .OrderByDescending(x => x.CalculateProbability(rules).Value) // TODO: What if CalculateProbability returns an error?
            .Take(numberOfQuestions ?? cards.Count)
            .Select((x, i) => new Question(Guid.NewGuid(), x.Id, i))
            .ToList();
}
