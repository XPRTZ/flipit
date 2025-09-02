using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Domain.CardAggregate.Rules;

public class AnsweredCorrectlyRule(IRandomNumberGenerator randomNumberGenerator) : IIncludeInQuizRule
{
    public int Weight => 4;
    
    public double CalculateProbability(Card card)
    {
        var baseValue = 1.0 - card.CorrectnessRatio;

        var noise = randomNumberGenerator.GetNoise(-0.1, 0.1);
         
        return Math.Clamp(baseValue + noise, 0.0, 1.0);
    }
}
