using FakeItEasy;
using Shouldly;
using Xprtz.FlipIt.Application.Quizzes.Commands;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Application.Tests.Quizzes.Commands;

public class CreateQuizCommandHandlerTests
{
    [Fact]
    public async Task Should_return_a_quiz_with_all_cards_if_number_of_questions_is_null()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork<Quiz>>();
        var rule = A.Fake<IIncludeInQuizRule>();

        var handler = new CreateQuizCommandHandler(unitOfWork, [rule]);

        int? numberOfQuestions = null;

        var cards = new List<Card>();
        for (var i = 0; i < 5; i++)
        {
            cards.Add(Card.Create(Guid.NewGuid(), $"Front{i}", $"Back{i}").Value);
        }

        var command = new CreateQuizCommand("Topic", numberOfQuestions, FrontOrBack.Back, true, cards);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.Questions.Count.ShouldBe(cards.Count);
    }

    [Fact]
    public async Task Should_return_a_quiz_with_the_number_of_questions_specified()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork<Quiz>>();
        var rule = A.Fake<IIncludeInQuizRule>();

        var handler = new CreateQuizCommandHandler(unitOfWork, [rule]);

        var numberOfQuestions = 2;

        var cards = new List<Card>();
        for (var i = 0; i < 5; i++)
        {
            cards.Add(Card.Create(Guid.NewGuid(), $"Front{i}", $"Back{i}").Value);
        }

        var command = new CreateQuizCommand("Topic", numberOfQuestions, FrontOrBack.Back, true, cards);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.Questions.Count.ShouldBe(numberOfQuestions);
    }

    [Fact]
    public async Task Should_return_an_error_when_there_are_no_questions_specified()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork<Quiz>>();
        var rule = A.Fake<IIncludeInQuizRule>();

        var handler = new CreateQuizCommandHandler(unitOfWork, [rule]);

        var command = new CreateQuizCommand("Topic", null, FrontOrBack.Back, true, []);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_return_a_quiz_with_the_right_questions_according_to_the_rules_engine()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork<Quiz>>();
        var rule = A.Fake<IIncludeInQuizRule>();
        A.CallTo(() => rule.Weight).Returns(1);

        var handler = new CreateQuizCommandHandler(unitOfWork, [rule]);

        var numberOfQuestions = 2;

        var cards = new List<Card>();
        for (var i = 0; i < 5; i++)
        {
            cards.Add(Card.Create(Guid.NewGuid(), $"Front{i}", $"Back{i}").Value);
        }
        
        var expectedCards = cards.Skip(3).Take(numberOfQuestions);
        A.CallTo(() => rule.CalculateProbability(A<Card>._))
            .ReturnsLazily(x => expectedCards.Contains(x.GetArgument<Card>(0)!) ? 1.0 : 0.0);

        var command = new CreateQuizCommand("Topic", numberOfQuestions, FrontOrBack.Back, true, cards);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.Questions.Count.ShouldBe(numberOfQuestions);
        foreach (var card in expectedCards)
        {
            result.Value.Questions.ShouldContain(x => x.CardId == card.Id);
        }
    }
}
