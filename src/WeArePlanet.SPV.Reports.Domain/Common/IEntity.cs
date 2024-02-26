using WeArePlanet.SPV.Reports.Domain.Events;

namespace WeArePlanet.SPV.Reports.Domain.Common;

public interface IEntity<out T, out TId> : IEntity<T>
    where T : IEntity<T>
    where TId : IId<T>
{
    new TId? Id { get; }
    object? IEntity<T>.Id => this.Id;
}

public interface IEntity<out T>
    where T : IEntity<T>
{
    object? Id { get; }

    IReadOnlyCollection<IDomainEvent<T>> DomainEvents { get; }
}
