using MediatR;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models.Extensions;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleArchivedEventHandler : INotificationHandler<ReportScheduleArchivedEvent>
{
    private readonly IReportScheduler scheduler;

    public ReportScheduleArchivedEventHandler(IReportScheduler scheduler)
    {
        this.scheduler = scheduler;
    }

    public async Task Handle(ReportScheduleArchivedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.PreviousState.IsActive())
        {
            await notification.Origin.DescheduleAsync(this.scheduler, cancellationToken);
        }
    }
}
