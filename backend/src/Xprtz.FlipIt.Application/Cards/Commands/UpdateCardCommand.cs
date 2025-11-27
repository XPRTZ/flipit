using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Specifications;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Cards.Commands;

public record UpdateCardCommand(Guid Id, Guid TopicId, string Front, string Back) : IRequest<ErrorOr<Card>>;

internal class UpdateCardCommandHandler(IUnitOfWork<Card> unitOfWork)
    : IRequestHandler<UpdateCardCommand, ErrorOr<Card>>
{
    public async Task<ErrorOr<Card>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var (id, topicId, front, back) = request;

        var card = await unitOfWork.Repository.FindOne(new HasTopicAndId(id, topicId), cancellationToken);

        if (card is null)
        {
            return Error.NotFound("Card not found");
        }

        var result = card.Update(front, back);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.SaveChanges(cancellationToken);

        return result.Value;
    }
}
