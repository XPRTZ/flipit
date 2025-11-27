using Xprtz.FlipIt.Domain.SeedWork.DomainEvents;

namespace Xprtz.FlipIt.Domain.SeedWork;

public abstract class AggregateRoot(Guid id) : Entity(id)
{
    private readonly List<DomainEvent> _domainEvents = [];

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public List<DomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}
