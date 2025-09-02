using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Cards.Queries;

public record GetCardQuery(Guid Id) : IRequest<ErrorOr<Card>>;

internal class GetCardQueryHandler(IUnitOfWork<Card> unitOfWork) : IRequestHandler<GetCardQuery, ErrorOr<Card>>
{
    public async Task<ErrorOr<Card>> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var card = await unitOfWork.Repository.Get(request.Id, cancellationToken);

        if (card is null)
        {
            return Error.NotFound("Card not found");
        }

        return card;
    }
}
