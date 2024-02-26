using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;

[ExcludeFromCodeCoverage]
public class ReportScheduleArchivedEvent : IDomainEvent<ReportSchedule>
{
    public ReportScheduleArchivedEvent(ReportSchedule reportSchedule, IReportScheduleState previousState,
        IDateTimeProvider dateTimeProvider)
    {
        this.Origin = reportSchedule;
        this.PreviousState = previousState;
        this.Id = Guid.NewGuid();
        this.TriggeredAt = dateTimeProvider.GetUtcNow().UtcDateTime;
    }

    public IReportScheduleState PreviousState { get; }

    public Guid Id { get; }
    public DateTime TriggeredAt { get; }
    public ReportSchedule Origin { get; }
}
