using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Topics.Commands;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public class Update(IRequestHandler<UpdateTopicCommand, ErrorOr<Domain.TopicAggregate.Topic>> handler)
    : Endpoint<TopicRequest, Topic?>
{
    public override void Configure()
    {
        Put("topic/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TopicRequest request, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(
            new(id, request.Name, request.FrontLabel, request.BackLabel),
            ct
        );

        await result.Match(
            async x => await SendOkAsync(x.ToContract(), ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(e => new ValidationFailure(e.Code, e.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
