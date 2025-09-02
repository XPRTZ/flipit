using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.ApiService.Tests;

public static class TestData
{
    public const string Card1 = "Front01";
    public const string Card2 = "Front02";
    public const string Card3 = "Front03";
    public const string Card4 = "Front04";
    public const string Card5 = "Front05";
    public const string Card6 = "Front06";
    public const string Card7 = "Front07";
    public const string Card8 = "Front08";
    public const string Card9 = "Front09";
    public const string Card10 = "Front10";

    private const int NumberOfCards = 10;

    internal static async Task SeedData(FlipItDbContext dbContext)
    {
        var topic = CreateTopic();
        dbContext.Topics.Add(topic);

        for (var i = 1; i <= NumberOfCards; i++)
        {
            var card = CreateCard(topic.Id, i);
            dbContext.Cards.Add(card);
        }

        await dbContext.SaveChangesAsync();
    }

    private static Topic CreateTopic() => Topic.Create("TestTopic", "Front", "Back").Value;

    private static Card CreateCard(Guid topicId, int index) =>
        Card.Create(topicId, $"Front{index:D2}", $"Back{index:D2}").Value;
}
