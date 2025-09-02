using Shouldly;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.Common;
using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;
using Xunit;

namespace Xprtz.FlipIt.ApiService.Tests.Endpoints.Quizzes;

public class CreateTests(FlipItApiFactory factory) : IClassFixture<FlipItApiFactory>, IAsyncLifetime
{
    private readonly IDateTimeProvider _dateTimeProvider = factory.DateTimeProvider;

    private readonly FlipItDbContext _dbContext = factory.DbContext;

    private readonly IQuizApi _quizApi = factory.QuizApi;

    [Fact]
    public async Task Should_create_a_quiz_with_all_cards_when_no_number_of_questions_is_specified()
    {
        // Arrange
        var topic = _dbContext.Topics.Single();

        var cards = _dbContext.Cards.Where(x => x.TopicId == topic.Id).ToList();

        var quizRequest = new QuizRequest
        {
            TopicId = topic.Id,
            TopicName = topic.Name,
            IsPracticeMode = true,
            NumberOfQuestions = null,
            ShowFirst = FrontOrBack.Back
        };

        // Act
        var quizResponse = await _quizApi.Create(quizRequest);

        // Assert
        var quiz = quizResponse.Content!;
        quiz.NumberOfQuestions.ShouldBe(cards.Count);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_unseen_cards_if_half_of_the_questions_are_seen()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 10 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 10 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 10 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 10 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 10 } },
                { TestData.Card6, new() { TimesSeen = 0 } },
                { TestData.Card7, new() { TimesSeen = 0 } },
                { TestData.Card8, new() { TimesSeen = 0 } },
                { TestData.Card9, new() { TimesSeen = 0 } },
                { TestData.Card10, new() { TimesSeen = 0 } }
            },
            ExpectedCards = [TestData.Card6, TestData.Card7, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_random_questions_if_all_cards_are_unseen()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 0 } },
                { TestData.Card2, new() { TimesSeen = 0 } },
                { TestData.Card3, new() { TimesSeen = 0 } },
                { TestData.Card4, new() { TimesSeen = 0 } },
                { TestData.Card5, new() { TimesSeen = 0 } },
                { TestData.Card6, new() { TimesSeen = 0 } },
                { TestData.Card7, new() { TimesSeen = 0 } },
                { TestData.Card8, new() { TimesSeen = 0 } },
                { TestData.Card9, new() { TimesSeen = 0 } },
                { TestData.Card10, new() { TimesSeen = 0 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card6, TestData.Card8, TestData.Card9]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_random_questions_if_all_cards_are_seen()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card6, TestData.Card8, TestData.Card9]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_longest_ago_seen_questions_if_all_cards_are_seen_at_different_dates()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 5 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 9 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 13 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 17 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 21 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 25 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 29 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 33 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 37 } }
            },
            ExpectedCards = [TestData.Card6, TestData.Card7, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task
        Should_create_a_quiz_with_a_mix_of_seen_and_unseen_questions_if_there_arent_enough_unseen_questions()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 0 } },
                { TestData.Card9, new() { TimesSeen = 0 } },
                { TestData.Card10, new() { TimesSeen = 0 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_random_questions_if_all_cards_have_been_answered_correctly()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card6, TestData.Card8, TestData.Card9]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_random_questions_if_all_cards_have_been_answered_incorrectly()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card6, TestData.Card8, TestData.Card9]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task
        Should_create_a_quiz_with_incorrectly_answered_questions_if_half_of_the_questions_are_answered_correctly()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } }
            },
            ExpectedCards = [TestData.Card6, TestData.Card7, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_most_incorrectly_answered_questions_if_correctness_ratio_differs()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card2, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card3, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card4, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card5, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card6, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card7, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card8, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card9, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } },
                { TestData.Card10, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = ViewedRecentlyRule.MaxDays } }
            },
            ExpectedCards = [TestData.Card6, TestData.Card7, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task
        Should_create_a_quiz_with_a_mix_of_correctly_and_incorrectly_answered_questions_if_there_arent_enough_questions_answered_correctly()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card2, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card3, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card6, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card8, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card9, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } },
                { TestData.Card10, new() { TimesSeen = 1, TimesCorrect = 0, LastSeen = 1 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card5, TestData.Card8, TestData.Card9, TestData.Card10]
        };
        
        await RunScenario(scenario);
    }

    [Fact]
    public async Task Should_create_a_quiz_with_expected_questions_given_a_certain_scenario()
    {
        var scenario = new TestScenario
        {
            CardTestData = new()
            {
                { TestData.Card1, new() { TimesSeen = 5, TimesCorrect = 3, LastSeen = 5 } },
                { TestData.Card2, new() { TimesSeen = 4, TimesCorrect = 4, LastSeen = 10 } },
                { TestData.Card3, new() { TimesSeen = 3, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card4, new() { TimesSeen = 5, TimesCorrect = 4, LastSeen = 1 } },
                { TestData.Card5, new() { TimesSeen = 1, TimesCorrect = 1, LastSeen = 5 } },
                { TestData.Card6, new() { TimesSeen = 2, TimesCorrect = 1, LastSeen = 1 } },
                { TestData.Card7, new() { TimesSeen = 3, TimesCorrect = 3, LastSeen = 5 } },
                { TestData.Card8, new() { TimesSeen = 5, TimesCorrect = 5, LastSeen = 10 } },
                { TestData.Card9, new() { TimesSeen = 3, TimesCorrect = 2, LastSeen = 5 } },
                { TestData.Card10, new() { TimesSeen = 2, TimesCorrect = 2, LastSeen = 10 } }
            },
            ExpectedCards = [TestData.Card1, TestData.Card3, TestData.Card4, TestData.Card6, TestData.Card9]
        };
        
        await RunScenario(scenario);
    }

    private async Task RunScenario(TestScenario scenario)
    {
        // Arrange
        var numberOfQuestions = 5;

        var topic = _dbContext.Topics.Single();

        await SetupTestScenario(topic.Id, scenario);

        var quizRequest = CreateRequest(topic, numberOfQuestions);

        // Act
        var quizResponse = await _quizApi.Create(quizRequest);

        // Assert
        var quiz = quizResponse.Content!;
        quiz.NumberOfQuestions.ShouldBe(numberOfQuestions);

        QuizQuestionsShouldMatchExpectation(quiz, scenario.ExpectedCards);
    }

    private static QuizRequest CreateRequest(Topic topic, int numberOfQuestions)
    {
        return new()
        {
            TopicId = topic.Id,
            TopicName = topic.Name,
            IsPracticeMode = true,
            NumberOfQuestions = numberOfQuestions,
            ShowFirst = FrontOrBack.Back
        };
    }

    private async Task SetupTestScenario(Guid topicId, TestScenario scenario)
    {
        foreach (var card in _dbContext.Cards.Where(x => x.TopicId == topicId))
        {
            if (!scenario.CardTestData.TryGetValue(card.Front, out var testData))
            {
                continue;
            }

            var numberOfTimesCorrect = 0;
            for (var i = 0; i < testData.TimesSeen; i++)
            {
                var correct = testData.TimesCorrect > numberOfTimesCorrect++;
                var lastViewed = _dateTimeProvider.Now.AddDays(-testData.LastSeen);
                card.Answered(correct, lastViewed);
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    private void QuizQuestionsShouldMatchExpectation(Quiz quiz, IEnumerable<string> expectedCardFronts)
    {
        var cardIdsInQuiz = _dbContext.Questions.Where(x => x.Quiz.Id == quiz.Id).Select(x => x.CardId).ToList();
        var cardFrontsInQuiz = _dbContext
            .Cards.Where(x => cardIdsInQuiz.Contains(x.Id))
            .Select(x => x.Front)
            .ToArray()
            .Order();
        cardFrontsInQuiz.ShouldBe(expectedCardFronts.ToArray().Order());
    }

    public Task InitializeAsync() => factory.SeedData();

    public Task DisposeAsync() => factory.ResetDatabase();
}
