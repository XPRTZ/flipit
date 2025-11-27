using System.Linq.Expressions;

namespace Xprtz.FlipIt.Domain.SeedWork.Specifications;

public class OrSpecification<TEntity> : Specification<TEntity> where TEntity : AggregateRoot
{
    private readonly Specification<TEntity> _left;
    private readonly Specification<TEntity> _right;

    public OrSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        var left = _left.ToExpression();
        var right = _right.ToExpression();
        
        var parameter = Expression.Parameter(typeof(TEntity));
        var expression = Expression.OrElse(left.Body, right.Body);
        
        expression = (BinaryExpression)new ParameterReplacer(parameter).Visit(expression);
        
        return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
    }
}
