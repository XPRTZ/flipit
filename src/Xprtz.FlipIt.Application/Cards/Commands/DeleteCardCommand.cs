using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.CardAggregate.Specifications;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Cards.Commands;

public record DeleteCardCommand(Guid Id, Guid TopicId) : IRequest<ErrorOr<Success>>;

internal class DeleteCardCommandHandler(IUnitOfWork<Card> unitOfWork)
    : IRequestHandler<DeleteCardCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var (id, topicId) = request;

        var card = await unitOfWork.Repository.FindOne(new HasTopicAndId(id, topicId), cancellationToken);

        if (card is null)
        {
            return Error.NotFound("Card not found");
        }

        card.Delete();

        await unitOfWork.Repository.Remove(card, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success;
    }
}
