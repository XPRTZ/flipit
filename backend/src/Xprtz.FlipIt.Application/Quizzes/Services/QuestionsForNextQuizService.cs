using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.QuizAggregate;

namespace Xprtz.FlipIt.Application.Quizzes.Services;

public class QuestionsForNextQuizService(IEnumerable<IIncludeInQuizRule> rules) : IQuestionsForNextQuizService
{
    public IReadOnlyCollection<Question> CreateQuestions(IReadOnlyCollection<Card> cards, int? numberOfQuestions) =>
        cards
            .OrderByDescending(x => CalculateProbability(x).Value) // TODO: What if CalculateProbability returns an error?
            .Take(numberOfQuestions ?? cards.Count)
            .Select((x, i) => new Question(Guid.NewGuid(), x.Id, i))
            .ToList();

    private ErrorOr<double> CalculateProbability(Card card)
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
            var p = rule.CalculateProbability(card);
            probability += p * rule.Weight;
            weight += rule.Weight;
        }

        if (weight == 0)
        {
            return Error.Validation(description: "Each rule should have a weight greater than zero.");
        }
        
        return probability / weight;
    }
}
