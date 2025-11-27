using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Specifications;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Cards.Queries;

public record GetAllCardsQuery(Guid TopicId) : IRequest<IReadOnlyCollection<Card>>;

internal class GetAllCardsQueryHandler(IUnitOfWork<Card> unitOfWork)
    : IRequestHandler<GetAllCardsQuery, IReadOnlyCollection<Card>>
{
    public async Task<IReadOnlyCollection<Card>> Handle(
        GetAllCardsQuery request,
        CancellationToken cancellationToken
    ) =>
        (await unitOfWork.Repository.Find(new HasTopic(request.TopicId), cancellationToken))
            .OrderBy(x => x.Front)
            .ToList();
}
