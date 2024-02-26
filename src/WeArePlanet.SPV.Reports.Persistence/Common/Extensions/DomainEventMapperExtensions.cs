using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;

namespace WeArePlanet.SPV.Reports.Persistence.Common.Extensions;

public static class DomainEventMapperExtensions
{
    public static EventDocument ToMongoDocument<T>(this IDomainEvent<T> domainEvent)
        where T : IEntity<T>
    {
        // TODO might want to find a better solution when the number of different event types grows, such as fetching a mapper from the IServiceProvider instance
        return domainEvent switch
        {
            ReportScheduleCreatedEvent reportScheduleCreatedEvent => new
                ReportScheduleCreatedEventDocument
                {
                    Id = reportScheduleCreatedEvent.Id.ToString(),
                    TriggeredAt = reportScheduleCreatedEvent.TriggeredAt,
                    ReportScheduleId = reportScheduleCreatedEvent.Origin.Id!.ToString()!
                },
            ReportScheduleUpdatedEvent reportScheduleUpdatedEvent => new
                ReportScheduleUpdatedEventDocument
                {
                    Id = reportScheduleUpdatedEvent.Id.ToString(),
                    TriggeredAt = reportScheduleUpdatedEvent.TriggeredAt,
                    ReportScheduleId = reportScheduleUpdatedEvent.Origin.Id!.ToString()!
                },
            ReportScheduleToggledEvent reportScheduleToggledEvent => new
                ReportScheduleToggledEventDocument
                {
                    Id = reportScheduleToggledEvent.Id.ToString(),
                    TriggeredAt = reportScheduleToggledEvent.TriggeredAt,
                    ReportScheduleId = reportScheduleToggledEvent.Origin.Id!.ToString()!,
                    Activated = reportScheduleToggledEvent.Origin.IsActive
                },
            ReportScheduleExecutedEvent reportScheduleExecutedEvent => new
                ReportScheduleExecutedEventDocument
                {
                    Id = reportScheduleExecutedEvent.Id.ToString(),
                    TriggeredAt = reportScheduleExecutedEvent.TriggeredAt,
                    ReportScheduleId = reportScheduleExecutedEvent.Origin.Id!.ToString()!
                },
            ReportScheduleArchivedEvent reportScheduleArchivedEvent => new
                ReportScheduleArchivedEventDocument
                {
                    Id = reportScheduleArchivedEvent.Id.ToString(),
                    TriggeredAt = reportScheduleArchivedEvent.TriggeredAt,
                    ReportScheduleId = reportScheduleArchivedEvent.Origin.Id!.ToString()!,
                    PreviousState = reportScheduleArchivedEvent.PreviousState.Name
                },
            _ => throw new InvalidOperationException(
                $"Event type [{domainEvent.GetType().Name}] is not mapped to a mongo document type.")
        };
    }
}
