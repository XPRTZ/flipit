using Xprtz.FlipIt.Domain.SeedWork.Rules;

namespace Xprtz.FlipIt.Domain.CardAggregate.Rules;

public interface IIncludeInQuizRule : IRule
{
    int Weight { get; }
    
    double CalculateProbability(Card card);
}
