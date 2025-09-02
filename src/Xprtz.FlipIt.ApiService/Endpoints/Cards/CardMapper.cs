using Card = Xprtz.FlipIt.Domain.CardAggregate.Card;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public static class CardMapper
{
    public static Contract.Cards.Card ToContract(this Card card) =>
        new()
        {
            Id = card.Id,
            Front = card.Front,
            Back = card.Back
        };

    public static IReadOnlyCollection<Contract.Cards.Card> ToContract(
        this IEnumerable<Card> cards
    ) => cards.Select(x => x.ToContract()).ToList();
}
