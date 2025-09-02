using FastEndpoints;
using Xprtz.FlipIt.Application.Cards.Queries;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public class GetById(IRequestHandler<GetCardQuery, ErrorOr<Domain.CardAggregate.Card>> handler)
    : EndpointWithoutRequest<Card>
{
    public override void Configure()
    {
        Get("topic/{topicId}/card/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var topicId = Route<Guid>("topicId");
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id), ct);

        await result.Match(
            async x =>
            {
                if (x.TopicId != topicId)
                {
                    await SendNotFoundAsync(ct);
                }
                else
                {
                    await SendOkAsync(x.ToContract(), ct);
                }
            },
            async _ => await SendNotFoundAsync(ct)
        );
    }
}
