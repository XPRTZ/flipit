using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Cards.Commands;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public class Create(IRequestHandler<CreateCardCommand, ErrorOr<Domain.CardAggregate.Card>> handler)
    : Endpoint<CardRequest, Card>
{
    public override void Configure()
    {
        Post("topic/{topicId}/card");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CardRequest request, CancellationToken ct)
    {
        var topicId = Route<Guid>("topicId");
        var result = await handler.Handle(new(topicId, request.Front, request.Back), ct);

        await result.Match(
            async x => await SendCreatedAtAsync<GetById>(new { id = x.Id, topicId }, x.ToContract(), cancellation: ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(e => new ValidationFailure(e.Code, e.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
