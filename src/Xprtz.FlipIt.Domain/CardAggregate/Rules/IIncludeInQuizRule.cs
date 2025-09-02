namespace Xprtz.FlipIt.Domain.CardAggregate.Rules;

public interface IIncludeInQuizRule
{
    int Weight { get; }
    
    double CalculateProbability(Card card);
}
