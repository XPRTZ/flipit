using FastEndpoints;
using Xprtz.FlipIt.Application.Topics.Queries;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public class GetById(IRequestHandler<GetTopicQuery, ErrorOr<Domain.TopicAggregate.Topic>> handler)
    : EndpointWithoutRequest<Topic>
{
    public override void Configure()
    {
        Get("topic/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id), ct);

        await result.Match(async x => await SendOkAsync(x.ToContract(), ct), async _ => await SendNotFoundAsync(ct));
    }
}
