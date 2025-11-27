using System.Linq.Expressions;

namespace Xprtz.FlipIt.Domain.SeedWork.Specifications;

internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    public ParameterReplacer(ParameterExpression parameter) => _parameter = parameter;

    protected override Expression VisitParameter(ParameterExpression node) => base.VisitParameter(_parameter);
}
