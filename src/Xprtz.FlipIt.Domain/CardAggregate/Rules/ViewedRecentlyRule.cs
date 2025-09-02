using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Domain.CardAggregate.Rules;

public class ViewedRecentlyRule(IRandomNumberGenerator randomNumberGenerator, IDateTimeProvider dateTimeProvider)
    : IIncludeInQuizRule
{
    public const int MaxDays = 50;

    public int Weight => 1;

    public double CalculateProbability(Card card)
    {
        var daysAgo = card.LastViewedAt is null ? MaxDays : (dateTimeProvider.Now - card.LastViewedAt.Value).TotalDays;

        var baseValue = daysAgo >= MaxDays ? 1.0 : daysAgo / MaxDays;
        
        var noise = randomNumberGenerator.GetNoise(-0.1, 0.1);
        
        return Math.Clamp(baseValue + noise, 0.0, 1.0);
    }
}
