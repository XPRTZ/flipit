using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Application.Cards.Rules;

public class NumberOfTimesSeenRule(IRandomNumberGenerator randomNumberGenerator) : IIncludeInQuizRule
{
    public int Weight => 2;

    public double CalculateProbability(Card card)
    {
        var baseValue = card.NumberOfViews switch
        {
            >= 10 => 0.2,
            >= 5 => 0.5,
            < 5 => 0.8
        };

        var noise = randomNumberGenerator.GetNoise(-0.2, 0.2);

        return Math.Clamp(baseValue + noise, 0.0, 1.0);
    }
}
