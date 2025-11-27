using System.Linq.Expressions;

namespace Xprtz.FlipIt.Domain.SeedWork.Specifications;

public class NotSpecification<TEntity> : Specification<TEntity> where TEntity : AggregateRoot
{
    private readonly Specification<TEntity> _original;

    public NotSpecification(Specification<TEntity> original) => _original = original;

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        var original = _original.ToExpression();
        
        var parameter = Expression.Parameter(typeof(TEntity));
        var expression = Expression.Not(original.Body);
        
        expression = (UnaryExpression)new ParameterReplacer(parameter).Visit(expression);
        
        return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
    }
}
