using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Topics.Commands;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Topics;

public class Create(IRequestHandler<CreateTopicCommand, ErrorOr<Domain.TopicAggregate.Topic>> handler)
    : Endpoint<TopicRequest, Topic>
{
    public override void Configure()
    {
        Post("topic");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TopicRequest request, CancellationToken ct)
    {
        var result = await handler.Handle(
            new(request.Name, request.FrontLabel, request.BackLabel),
            ct
        );

        await result.Match(
            async x => await SendCreatedAtAsync<GetById>(new { id = x.Id }, x.ToContract(), cancellation: ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(e => new ValidationFailure(e.Code, e.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
