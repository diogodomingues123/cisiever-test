using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public interface IReportExecutionPlan : IEntity<IReportExecutionPlan, ExecutionPlanId>
{
    Task<ExecutionPlanId> ScheduleAsync(ReportSchedule schedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken = default);

    Task DescheduleAsync(ReportSchedule reportSchedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken);

    IReportScheduleState EvaluateStateTransition(ReportSchedule schedule, IReportScheduleState desiredState);

    DateTime? GetNextExecutionDateInUtc(IDateTimeProvider dateTimeProvider);
}
