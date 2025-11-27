namespace Xprtz.FlipIt.Domain.SeedWork;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; } = id;

    public bool Equals(Entity? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) => obj is Entity other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode() ^ 31;

    public static bool operator ==(Entity? left, Entity? right) =>
        (left is null && right is null) || (left?.Equals(right) ?? false);

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}
