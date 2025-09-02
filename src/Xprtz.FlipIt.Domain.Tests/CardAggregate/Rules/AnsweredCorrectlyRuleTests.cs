using FakeItEasy;
using Shouldly;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Domain.Tests.CardAggregate.Rules;

public class AnsweredCorrectlyRuleTests
{
    [Fact]
    public void Should_return_maximum_value_when_not_answered_at_all()
    {
        // Arrange
        var randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
        A.CallTo(() => randomNumberGenerator.GetNoise(A<double>._, A<double>._)).Returns(-0.05);

        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;
        
        var rule = new AnsweredCorrectlyRule(randomNumberGenerator);
            
        // Act
        var result = rule.CalculateProbability(card);

        // Assert
        result.ShouldBe(0.95, tolerance: 0.0001);
    }
    
    [Fact]
    public void Should_return_minimum_value_when_answered_correctly_every_time()
    {
        // Arrange
        var randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
        A.CallTo(() => randomNumberGenerator.GetNoise(A<double>._, A<double>._)).Returns(0.05);

        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;
        card.Answered(true, new(2000,1,1));
        
        var rule = new AnsweredCorrectlyRule(randomNumberGenerator);
            
        // Act
        var result = rule.CalculateProbability(card);

        // Assert
        result.ShouldBe(0.05, tolerance: 0.0001);
    }
    
    [Theory]
    [InlineData(2, 1, 0.05,0.55)]
    [InlineData(2, 1, -0.05, 0.45)]
    [InlineData(4, 1, 0.05,0.8)]
    [InlineData(4, 3, -0.05,0.2)]
    public void Should_return_expected_value_when_answered_correctly_several_times(int timesSeen, int timesCorrect, double noise, double expectedProbability)
    {
        // Arrange
        var randomNumberGenerator = A.Fake<IRandomNumberGenerator>();
        A.CallTo(() => randomNumberGenerator.GetNoise(A<double>._, A<double>._)).Returns(noise);

        var date = new DateTime(2000, 1, 1);
        
        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;
        for (var i = 0; i < timesSeen; i++)
        {
            card.Answered(i < timesCorrect, date);
        }
        
        var rule = new AnsweredCorrectlyRule(randomNumberGenerator);
            
        // Act
        var result = rule.CalculateProbability(card);

        // Assert
        result.ShouldBe(expectedProbability, tolerance: 0.0001);
    }
}
