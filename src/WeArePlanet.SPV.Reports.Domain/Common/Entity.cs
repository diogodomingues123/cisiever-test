using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Events;

namespace WeArePlanet.SPV.Reports.Domain.Common;

[ExcludeFromCodeCoverage]
public abstract class Entity<T, TId> : IEntity<T, TId>, IComponentEquatable<T>
    where T : Entity<T, TId>
    where TId : IId<T>
{
    private readonly List<IDomainEvent<T>> domainEvents;

    private Entity()
    {
        this.domainEvents = new List<IDomainEvent<T>>();
    }

    protected Entity(TId? id)
        : this()
    {
        this.Id = id;
    }

    public IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Id;
    }

    public bool Equals(T? other)
    {
        return this.Equals(other as Entity<T, TId>);
    }

    public TId? Id { get; }

    public IReadOnlyCollection<IDomainEvent<T>> DomainEvents => this.domainEvents.AsReadOnly();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == this.GetType() && this.Equals((Entity<T, TId>)obj);
    }

    public override int GetHashCode()
    {
        return this.GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private bool Equals(Entity<T, TId>? other)
    {
        if (other == null)
        {
            return false;
        }

        return this.GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    protected void RaiseEvent(IDomainEvent<T> domainEvent)
    {
        this.domainEvents.Add(domainEvent);
    }
}
