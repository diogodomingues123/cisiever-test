using MediatR;

using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime TriggeredAt { get; }
}

public interface IDomainEvent<out T> : IDomainEvent
    where T : IEntity<T>
{
    T Origin { get; }
}
