using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;

[ExcludeFromCodeCoverage]
public class ReportScheduleToggledEvent : IDomainEvent<ReportSchedule>
{
    public ReportScheduleToggledEvent(ReportSchedule reportSchedule, IDateTimeProvider dateTimeProvider)
    {
        this.Origin = reportSchedule;
        this.Id = Guid.NewGuid();
        this.TriggeredAt = dateTimeProvider.GetUtcNow().UtcDateTime;
    }

    public Guid Id { get; }
    public DateTime TriggeredAt { get; }
    public ReportSchedule Origin { get; }
}
