namespace Xprtz.FlipIt.Contract.Cards;

public class Card
{
    public required Guid Id { get; init; }
    public required string Front { get; init; }
    public required string Back { get; init; }
}
