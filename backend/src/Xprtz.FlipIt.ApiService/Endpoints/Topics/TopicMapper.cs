using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public static class TopicMapper
{
    public static Contract.Topics.Topic ToContract(this Topic topic) =>
        new()
        {
            Id = topic.Id,
            Name = topic.Name,
            FrontLabel = topic.FrontLabel,
            BackLabel = topic.BackLabel
        };

    public static IReadOnlyCollection<Contract.Topics.Topic> ToContract(
        this IEnumerable<Topic> subjects
    ) => subjects.Select(x => x.ToContract()).ToList();
}
