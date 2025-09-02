using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Cards.Commands;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Cards;

public class Update(IRequestHandler<UpdateCardCommand, ErrorOr<Domain.CardAggregate.Card>> handler)
    : Endpoint<CardRequest, Card?>
{
    public override void Configure()
    {
        Put("topic/{topicId}/card/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CardRequest request, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var topicId = Route<Guid>("topicId");
        var result = await handler.Handle(new(id, topicId, request.Front, request.Back), ct);

        await result.Match(
            async x => await SendOkAsync(x.ToContract(), ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(x => new ValidationFailure(x.Code, x.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
