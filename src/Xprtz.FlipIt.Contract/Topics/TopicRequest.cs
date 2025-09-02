namespace Xprtz.FlipIt.Contract.Topics;

public class TopicRequest
{
    public required string Name { get; init; }
    public required string FrontLabel { get; init; }
    public required string BackLabel { get; init; }
}
