using MediatR;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleToggledEventHandler : INotificationHandler<ReportScheduleToggledEvent>
{
    private readonly IReportScheduler scheduler;

    public ReportScheduleToggledEventHandler(IReportScheduler scheduler)
    {
        this.scheduler = scheduler;
    }

    public async Task Handle(ReportScheduleToggledEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Origin.IsActive)
        {
            await notification.Origin.ScheduleAsync(this.scheduler, cancellationToken);
        }
        else
        {
            await notification.Origin.DescheduleAsync(this.scheduler, cancellationToken);
        }
    }
}
