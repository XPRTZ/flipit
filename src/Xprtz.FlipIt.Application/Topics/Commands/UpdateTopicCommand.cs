using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Application.Topics.Commands;

public record UpdateTopicCommand(Guid Id, string Name, string FrontLabel, string BackLabel) : IRequest<ErrorOr<Topic>>;

internal class UpdateTopicCommandHandler(IUnitOfWork<Topic> unitOfWork)
    : IRequestHandler<UpdateTopicCommand, ErrorOr<Topic>>
{
    public async Task<ErrorOr<Topic>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var (id, name, frontLabel, backLabel) = request;

        var topic = await unitOfWork.Repository.Get(id, cancellationToken);

        if (topic is null)
        {
            return Error.NotFound("Topic not found");
        }

        var result = topic.Update(name, frontLabel, backLabel);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.SaveChanges(cancellationToken);

        return result.Value;
    }
}
