using Cronos;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class RecurringReportExecutionPlan : ValueObject<RecurringReportExecutionPlan>, IReportExecutionPlan
{
    public RecurringReportExecutionPlan(string frequency, TimeZoneId? timeZone)
    {
        this.Frequency = frequency;
        this.TimeZoneId = timeZone ?? TimeZoneId.Utc;
    }

    public RecurringReportExecutionPlan(ExecutionPlanId id, string frequency, TimeZoneId? timeZone)
        : this(frequency, timeZone)
    {
        this.Id = id;
    }

    public string Frequency { get; }

    public TimeZoneId TimeZoneId { get; }

    public ExecutionPlanId? Id { get; private set; }

    public IReadOnlyCollection<IDomainEvent<IReportExecutionPlan>> DomainEvents { get; } =
        Enumerable.Empty<IDomainEvent<IReportExecutionPlan>>().ToList();

    public async Task<ExecutionPlanId> ScheduleAsync(ReportSchedule schedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken = default)
    {
        this.Id = await reportScheduler.ScheduleRecurringAsync(
            schedule.Id!,
            this.Frequency,
            this.TimeZoneId,
            cancellationToken);

        return this.Id;
    }

    public async Task DescheduleAsync(ReportSchedule reportSchedule, IReportScheduler reportScheduler,
        CancellationToken cancellationToken)
    {
        if (this.Id == null)
        {
            throw new InvalidOperationException("Execution Plan Id cannot be unset.");
        }

        await reportScheduler.DescheduleRecurringAsync(this.Id, cancellationToken);
    }

    public IReportScheduleState EvaluateStateTransition(ReportSchedule schedule, IReportScheduleState desiredState)
    {
        // Recurring jobs can never be in the executed state since they are, you guessed it, recurring.
        // It makes sense to keep them either active or inactive (or other future states, to be checked) and keep track of their executions.
        return desiredState is ExecutedReportScheduleState ? schedule.State : desiredState;
    }

    public DateTime? GetNextExecutionDateInUtc(IDateTimeProvider dateTimeProvider)
    {
        var cronExpression = CronExpression.Parse(this.Frequency);

        if (!this.TimeZoneId.Equals(TimeZoneId.Utc))
        {
            return cronExpression
                .GetNextOccurrence(dateTimeProvider.GetUtcNow(), this.TimeZoneId)
                .GetValueOrDefault()
                .UtcDateTime;
        }

        return cronExpression.GetNextOccurrence(dateTimeProvider.GetUtcNow().UtcDateTime);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Frequency;
    }
}
