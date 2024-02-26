using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleUpdatedEventHandler : INotificationHandler<ReportScheduleUpdatedEvent>
{
    public Task Handle(ReportScheduleUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Nothing to do (yet)
        return Task.CompletedTask;
    }
}
