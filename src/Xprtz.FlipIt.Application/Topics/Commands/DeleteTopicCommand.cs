using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Application.Topics.Commands;

public record DeleteTopicCommand(Guid Id) : IRequest<ErrorOr<Success>>;

internal class DeleteTopicCommandHandler(IUnitOfWork<Topic> unitOfWork)
    : IRequestHandler<DeleteTopicCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await unitOfWork.Repository.Get(request.Id, cancellationToken);

        if (topic is null)
        {
            return Error.NotFound("Topic not found");
        }

        topic.Delete();

        await unitOfWork.Repository.Remove(topic, cancellationToken);

        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success;
    }
}
