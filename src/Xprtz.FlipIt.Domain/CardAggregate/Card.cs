using ErrorOr;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Domain.CardAggregate;

public class Card : AggregateRoot
{
    public Guid TopicId { get; private set; }
    public string Front { get; private set; }
    public string Back { get; private set; }
    public int NumberOfViews { get; private set; }
    public int NumberOfCorrectAnswers { get; private set; }
    public double CorrectnessRatio => NumberOfViews == 0 ? 0 : (double)NumberOfCorrectAnswers / NumberOfViews;
    public DateTime? LastViewedAt { get; private set; }

    private Card(Guid id, Guid topicId, string front, string back)
        : base(id)
    {
        TopicId = topicId;
        Front = front;
        Back = back;
    }

    public ErrorOr<double> CalculateProbability(IEnumerable<IIncludeInQuizRule> rules)
    {
        var r = rules.ToList();
        if (r.Count == 0)
        {
            return Error.Validation(description: "Rules are empty.");
        }
        
        double probability = 0;
        double weight = 0;
        foreach (var rule in r)
        {
            var p = rule.CalculateProbability(this);
            probability += p * rule.Weight;
            weight += rule.Weight;
        }

        if (weight == 0)
        {
            return Error.Validation(description: "Each rule should have a weight greater than zero.");
        }
        
        return probability / weight;
    }

    public ErrorOr<Card> Update(string front, string back)
    {
        var errors = Validate(TopicId, front, back);

        if (errors.Count != 0)
        {
            return errors;
        }

        Front = front;
        Back = back;

        return this;
    }

    public ErrorOr<Success> Answered(bool isCorrect, DateTime lastViewedAt)
    {
        NumberOfViews++;

        if (isCorrect)
        {
            NumberOfCorrectAnswers++;
        }

        LastViewedAt = lastViewedAt;

        return Result.Success;
    }

    public ErrorOr<Success> Delete()
    {
        // nothing for now, raise domain event later
        return Result.Success;
    }

    public static ErrorOr<Card> Create(Guid topicId, string front, string back)
    {
        var errors = Validate(topicId, front, back);

        if (errors.Count != 0)
        {
            return errors;
        }

        return new Card(Guid.NewGuid(), topicId, front, back);
    }

    private static List<Error> Validate(Guid topicId, string front, string back)
    {
        List<Error> errors = [];

        if (topicId == Guid.Empty)
        {
            errors.Add(Error.Validation("Topic Id is required"));
        }

        if (string.IsNullOrWhiteSpace(front))
        {
            errors.Add(Error.Validation("Front text is required"));
        }

        if (string.IsNullOrWhiteSpace(back))
        {
            errors.Add(Error.Validation("Back text is required"));
        }

        return errors;
    }
}
