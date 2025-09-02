namespace Xprtz.FlipIt.Domain.SeedWork.DomainEvents;

public abstract record DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
