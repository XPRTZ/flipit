using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Application.Topics.Commands;

public record CreateTopicCommand(string Name, string FrontLabel, string BackLabel) : IRequest<ErrorOr<Topic>>;

internal class CreateTopicCommandHandler(IUnitOfWork<Topic> unitOfWork)
    : IRequestHandler<CreateTopicCommand, ErrorOr<Topic>>
{
    public async Task<ErrorOr<Topic>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var (name, frontLabel, backLabel) = request;

        var result = Topic.Create(name, frontLabel, backLabel);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.Repository.Add(result.Value, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}
