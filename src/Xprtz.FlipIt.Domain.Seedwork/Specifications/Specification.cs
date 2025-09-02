using System.Linq.Expressions;

namespace Xprtz.FlipIt.Domain.SeedWork.Specifications;

public abstract class Specification<TEntity> where TEntity : AggregateRoot
{
    public abstract Expression<Func<TEntity, bool>> ToExpression();
    
    public bool IsSatisfiedBy(TEntity entity) => ToExpression().Compile()(entity);
    
    public Specification<TEntity> And(Specification<TEntity> specification) => new AndSpecification<TEntity>(this, specification);
    public Specification<TEntity> Or(Specification<TEntity> specification) => new OrSpecification<TEntity>(this, specification);
    public Specification<TEntity> Xor(Specification<TEntity> specification) => new XorSpecification<TEntity>(this, specification);
    public Specification<TEntity> Not() => new NotSpecification<TEntity>(this);
    
    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) => specification.ToExpression();
    
    public static Specification<TEntity> operator &(Specification<TEntity> left, Specification<TEntity> right) => left.And(right);
    public static Specification<TEntity> operator |(Specification<TEntity> left, Specification<TEntity> right) => left.Or(right);
    public static Specification<TEntity> operator ^(Specification<TEntity> left, Specification<TEntity> right) => left.Xor(right);
    public static Specification<TEntity> operator !(Specification<TEntity> specification) => specification.Not();
}
