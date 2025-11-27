using ErrorOr;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Domain.QuizAggregate;

public class Question(Guid id, Guid cardId, int order) : Entity(id)
{
    public Quiz Quiz { get; private set; } = null!;

    public Guid CardId { get; } = cardId;
    public int Order { get; } = order;
    public bool IsAnswered { get; private set; }
    public bool IsCorrect { get; private set; }

    public ErrorOr<Success> Answer(bool isCorrect)
    {
        IsAnswered = true;
        IsCorrect = isCorrect;

        return Result.Success;
    }
}
