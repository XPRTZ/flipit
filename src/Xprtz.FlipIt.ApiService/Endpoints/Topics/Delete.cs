using FastEndpoints;
using Xprtz.FlipIt.Application.Topics.Commands;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public class Delete(IRequestHandler<DeleteTopicCommand, ErrorOr<Success>> handler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("topic/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id), ct);

        await result.Match(async _ => await SendNoContentAsync(ct), async _ => await SendNotFoundAsync(ct));
    }
}
