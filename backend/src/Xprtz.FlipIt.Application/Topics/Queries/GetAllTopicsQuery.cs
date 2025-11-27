using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Application.Topics.Queries;

public record GetAllTopicsQuery() : IRequest<IReadOnlyCollection<Topic>>;

internal class GetAllTopicsQueryHandler(IUnitOfWork<Topic> unitOfWork)
    : IRequestHandler<GetAllTopicsQuery, IReadOnlyCollection<Topic>>
{
    public async Task<IReadOnlyCollection<Topic>> Handle(
        GetAllTopicsQuery request,
        CancellationToken cancellationToken
    ) => (await unitOfWork.Repository.GetAll(cancellationToken)).OrderBy(x => x.Name).ToList();
}
