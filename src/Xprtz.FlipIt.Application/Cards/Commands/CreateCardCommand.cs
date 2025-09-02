using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application.Cards.Commands;

public record CreateCardCommand(Guid TopicId, string Front, string Back) : IRequest<ErrorOr<Card>>;

internal class CreateCardCommandHandler(IUnitOfWork<Card> unitOfWork)
    : IRequestHandler<CreateCardCommand, ErrorOr<Card>>
{
    public async Task<ErrorOr<Card>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var (topicId, front, back) = request;

        var result = Card.Create(topicId, front, back);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.Repository.Add(result.Value, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}
