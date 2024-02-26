using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleCreatedEventHandler : INotificationHandler<ReportScheduleCreatedEvent>
{
    public Task Handle(ReportScheduleCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Nothing to do (yet)
        return Task.CompletedTask;
    }
}
