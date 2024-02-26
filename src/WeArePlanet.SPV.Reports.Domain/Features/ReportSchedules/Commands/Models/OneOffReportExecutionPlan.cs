using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class OneOffReportExecutionPlan : ValueObject<OneOffReportExecutionPlan>, IReportExecutionPlan
{
    public OneOffReportExecutionPlan()
        : this(default, default)
    {
    }

    public OneOffReportExecutionPlan(DateTime? date, TimeZoneId? timeZone)
    {
        this.Date = date;
        this.TimeZoneId = timeZone ?? TimeZoneId.Utc;
    }

    public OneOffReportExecutionPlan(ExecutionPlanId id, DateTime? date = default, TimeZoneId? timeZone = default)
        : this(date, timeZone)
    {
        this.Id = id;
    }

    public DateTime? Date { get; }

    public TimeZoneId TimeZoneId { get; }

    public ExecutionPlanId? Id { get; private set; }

    public IReadOnlyCollection<IDomainEvent<IReportExecutionPlan>> DomainEvents { get; } =
        Enumerable.Empty<IDomainEvent<IReportExecutionPlan>>().ToList();


    public async Task<ExecutionPlanId> ScheduleAsync(ReportSchedule schedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken = default)
    {
        if (this.Date.HasValue)
        {
            this.Id = await reportScheduler.ScheduleAsync(schedule.Id!, this.Date.Value, this.TimeZoneId,
                cancellationToken);
        }
        else
        {
            this.Id = await reportScheduler.TriggerAsync(schedule.Id!, cancellationToken);
        }

        return this.Id;
    }

    public async Task DescheduleAsync(ReportSchedule reportSchedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken)
    {
        if (this.Id == null)
        {
            throw new InvalidOperationException("Execution Plan Id cannot be unset.");
        }

        await reportScheduler.DescheduleAsync(this.Id, cancellationToken);
    }

    public IReportScheduleState EvaluateStateTransition(ReportSchedule schedule, IReportScheduleState desiredState)
    {
        return desiredState;
    }

    public DateTime? GetNextExecutionDateInUtc(IDateTimeProvider dateTimeProvider)
    {
        if (!this.Date.HasValue)
        {
            return dateTimeProvider.GetUtcNow().UtcDateTime;
        }

        return dateTimeProvider.ConvertToUtc(this.Date.Value, this.TimeZoneId);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Date;
    }
}
