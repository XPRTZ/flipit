using FakeItEasy;
using Shouldly;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;

namespace Xprtz.FlipIt.Domain.Tests.CardAggregate;

public class CardTests
{
    [Fact]
    public void CalculateProbability_should_return_error_if_there_are_no_rules()
    {
        // Arrange
        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;

        // Act
        var probability = card.CalculateProbability([]);

        // Assert
        probability.IsError.ShouldBeTrue();
        probability.FirstError.Description.ShouldContain("Rules are empty");
    }

    [Fact]
    public void CalculateProbability_should_return_error_if_rules_have_no_weights()
    {
        // Arrange
        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;
        var rule = A.Fake<IIncludeInQuizRule>();

        // Act
        var probability = card.CalculateProbability([rule]);

        // Assert
        probability.IsError.ShouldBeTrue();
        probability.FirstError.Description.ShouldContain("Each rule should have a weight");
    }

    [Fact]
    public void CalculateProbability_should_return_probability_of_a_rule_when_it_is_the_only_rule()
    {
        // Arrange
        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;

        var expectedProbability = 0.5;
        var rule = A.Fake<IIncludeInQuizRule>();
        A.CallTo(() => rule.Weight).Returns(1);
        A.CallTo(() => rule.CalculateProbability(card)).Returns(expectedProbability);

        // Act
        var probability = card.CalculateProbability([rule]);

        // Assert
        probability.IsError.ShouldBeFalse();
        probability.Value.ShouldBe(expectedProbability);
    }

    [Fact]
    public void CalculateProbability_should_return_probability_times_weight()
    {
        // Arrange
        var card = Card.Create(Guid.NewGuid(), "Front", "Back").Value;

        var rule1Weight = 1;
        var rule1Probability = 0.5;
        var rule2Weight = 2;
        var rule2Probability = 0.1;

        var expectedProbability = 0.7 / 3;
        // (rule1Probability * rule1Weight + rule2Probability * rule2Weight) / (rule1Weight + rule2Weight)

        var rule1 = A.Fake<IIncludeInQuizRule>();
        A.CallTo(() => rule1.Weight).Returns(rule1Weight);
        A.CallTo(() => rule1.CalculateProbability(card)).Returns(rule1Probability);

        var rule2 = A.Fake<IIncludeInQuizRule>();
        A.CallTo(() => rule2.Weight).Returns(rule2Weight);
        A.CallTo(() => rule2.CalculateProbability(card)).Returns(rule2Probability);

        // Act
        var probability = card.CalculateProbability([rule1, rule2]);

        // Assert
        probability.IsError.ShouldBeFalse();
        probability.Value.ShouldBe(expectedProbability);
    }
}
