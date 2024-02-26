using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public interface IReportScheduler
{
    Task<ExecutionPlanId> ScheduleAsync(
        ReportScheduleId reportScheduleId,
        DateTime scheduleTo,
        TimeZoneId timezone,
        CancellationToken cancellationToken = default);

    Task<ExecutionPlanId> ScheduleRecurringAsync(
        ReportScheduleId reportScheduleId,
        string frequency,
        TimeZoneId timeZoneId,
        CancellationToken cancellationToken = default);

    Task<ExecutionPlanId> TriggerAsync(
        ReportScheduleId reportScheduleId,
        CancellationToken cancellationToken = default);

    Task DescheduleAsync(ExecutionPlanId id, CancellationToken cancellationToken = default);

    Task DescheduleRecurringAsync(ExecutionPlanId id, CancellationToken cancellationToken = default);
}
