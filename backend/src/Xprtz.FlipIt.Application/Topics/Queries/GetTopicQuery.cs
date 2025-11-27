using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Application.Topics.Queries;

public record GetTopicQuery(Guid Id) : IRequest<ErrorOr<Topic>>;

internal class GetTopicQueryHandler(IUnitOfWork<Topic> unitOfWork) : IRequestHandler<GetTopicQuery, ErrorOr<Topic>>
{
    public async Task<ErrorOr<Topic>> Handle(GetTopicQuery request, CancellationToken cancellationToken)
    {
        var topic = await unitOfWork.Repository.Get(request.Id, cancellationToken);

        if (topic is null)
        {
            return Error.NotFound("Topic not found");
        }

        return topic;
    }
}
