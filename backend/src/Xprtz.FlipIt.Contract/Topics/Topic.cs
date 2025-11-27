namespace Xprtz.FlipIt.Contract.Topics;

public class Topic
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string FrontLabel { get; init; }
    public required string BackLabel { get; init; }
}
