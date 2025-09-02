using FastEndpoints;
using Xprtz.FlipIt.Application.Cards.Queries;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public class Get(IRequestHandler<GetAllCardsQuery, IReadOnlyCollection<Domain.CardAggregate.Card>> handler)
    : EndpointWithoutRequest<IReadOnlyCollection<Card>>
{
    public override void Configure()
    {
        Get("topic/{topicId}/card");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var topicId = Route<Guid>("topicId");
        var result = await handler.Handle(new(topicId), ct);

        await SendOkAsync(result.ToContract(), ct);
    }
}
