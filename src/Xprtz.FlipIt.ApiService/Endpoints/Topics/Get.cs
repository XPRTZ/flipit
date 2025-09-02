using FastEndpoints;
using Xprtz.FlipIt.Application.Topics.Queries;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public class Get(IRequestHandler<GetAllTopicsQuery, IReadOnlyCollection<Domain.TopicAggregate.Topic>> handler)
    : EndpointWithoutRequest<IReadOnlyCollection<Topic>>
{
    public override void Configure()
    {
        Get("topic");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await handler.Handle(new(), ct);

        await SendOkAsync(result.ToContract(), ct);
    }
}
