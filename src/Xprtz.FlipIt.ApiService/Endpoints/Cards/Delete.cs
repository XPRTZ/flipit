using FastEndpoints;
using Xprtz.FlipIt.Application.Cards.Commands;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public class Delete(IRequestHandler<DeleteCardCommand, ErrorOr<Success>> handler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("topic/{topicId}/card/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var topicId = Route<Guid>("topicId");
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id, topicId), ct);

        await result.Match(async _ => await SendNoContentAsync(ct), async _ => await SendNotFoundAsync(ct));
    }
}
