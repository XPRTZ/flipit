using FakeItEasy;
using Shouldly;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Domain.Tests.CardAggregate.Rules;

public class ViewedRecentlyRuleTests
{
    [Theory]
    [InlineData(1, -0.05, 0.0)]
    [InlineData(1, 0.05, 0.07)]
    [InlineData(2, 0.05, 0.09)]
    [InlineData(4, 0.05, 0.13)]
    [InlineData(8, 0.05, 0.21)]
    [InlineData(16, 0.05, 0.37)]
    [InlineData(32, 0.05, 0.69)]
    [InlineData(50, -0.05, 0.95)]
    [InlineData(50, 0.05, 1.0)]
    public void Should_return_expected_value_when_answered_correctly_several_times(
        int daysAgo,
        double noise,
        double expectedProbability)
    {
        // Arrange
        var now = new DateTime(2000, 1, 1);

        var dateTimeProvider = A.Fake<IDateTimeProvider>();
        A.CallTo(() => dateTimeProvider.Now).Returns(now);

        var randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
        A.CallTo(() => randomNumberGenerator.GetNoise(A<double>._, A<double>._)).Returns(noise);

        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;
        card.Answered(true, now.AddDays(-1 * daysAgo));

        var rule = new ViewedRecentlyRule(randomNumberGenerator, dateTimeProvider);

        // Act
        var result = rule.CalculateProbability(card);

        // Assert
        result.ShouldBe(expectedProbability, tolerance: 0.0001);
    }
}
