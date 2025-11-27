using System.Linq.Expressions;
using Xprtz.FlipIt.Domain.SeedWork.Specifications;

namespace Xprtz.FlipIt.Domain.CardAggregate.Specifications;

public class HasTopic(Guid topicId) : Specification<Card>
{
    public override Expression<Func<Card, bool>> ToExpression() => c => c.TopicId == topicId;
}
