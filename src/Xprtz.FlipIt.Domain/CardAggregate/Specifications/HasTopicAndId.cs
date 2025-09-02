using System.Linq.Expressions;
using Xprtz.FlipIt.Domain.SeedWork.Specifications;

namespace Xprtz.FlipIt.Domain.CardAggregate.Specifications;

public class HasTopicAndId(Guid id, Guid topicId) : Specification<Card>
{
    public override Expression<Func<Card, bool>> ToExpression() =>
        c => c.Id == id && c.TopicId == topicId;
}
