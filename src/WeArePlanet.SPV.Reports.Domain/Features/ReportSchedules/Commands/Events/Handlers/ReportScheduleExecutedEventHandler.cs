using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleExecutedEventHandler : INotificationHandler<ReportScheduleExecutedEvent>
{
    public Task Handle(ReportScheduleExecutedEvent notification, CancellationToken cancellationToken)
    {
        // Nothing to do (yet)
        return Task.CompletedTask;
    }
}
