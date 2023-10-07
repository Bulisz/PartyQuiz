namespace Domain.Entities.Base;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; private init; }

    protected BaseEntity(Guid id) => Id = id;

    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        if (obj is not BaseEntity baseEntity)
            return false;

        return baseEntity.Id == Id;
    }

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return other.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
