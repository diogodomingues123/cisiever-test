using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Hangfire;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportExecutions;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

// TODO: Remove this attribute once we have integration tests.
[ExcludeFromCodeCoverage(Justification =
    "It is very complex to mock hangfire interfaces because they rely on extension methods. Should be tested via integration tests.")]
public class HangfireReportScheduler : IReportScheduler
{
    private readonly IBackgroundJobClient backgroundJobClient;
    private readonly IRecurringJobManager recurringJobManager;

    public HangfireReportScheduler(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
    {
        this.backgroundJobClient = backgroundJobClient;
        this.recurringJobManager = recurringJobManager;
    }

    public Task<ExecutionPlanId> ScheduleAsync(
        ReportScheduleId reportScheduleId,
        DateTime scheduleTo,
        TimeZoneId timeZoneId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tzInfo = (TimeZoneInfo)timeZoneId;
        var convertedScheduleToDate = TimeZoneInfo.ConvertTimeToUtc(scheduleTo, tzInfo);

        var scheduleId = this.backgroundJobClient.Schedule(
            ExecuteReportJob(reportScheduleId),
            convertedScheduleToDate);

        return Task.FromResult(new ExecutionPlanId(scheduleId));
    }

    public Task<ExecutionPlanId> ScheduleRecurringAsync(
        ReportScheduleId reportScheduleId,
        string frequency,
        TimeZoneId timeZoneId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        this.recurringJobManager.AddOrUpdate(
            reportScheduleId.ToString(),
            ExecuteReportJob(reportScheduleId),
            frequency,
            new RecurringJobOptions { TimeZone = timeZoneId, MisfireHandling = MisfireHandlingMode.Relaxed });

        return Task.FromResult(new ExecutionPlanId(reportScheduleId.ToString()!));
    }

    public Task<ExecutionPlanId> TriggerAsync(ReportScheduleId reportScheduleId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var scheduleId =
            this.backgroundJobClient.Enqueue<CreateReportJob>(job =>
                ExecuteReportJob(reportScheduleId));

        return Task.FromResult(new ExecutionPlanId(scheduleId));
    }

    public Task DescheduleAsync(ExecutionPlanId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        this.backgroundJobClient.Delete(id.ToString());

        return Task.CompletedTask;
    }

    public Task DescheduleRecurringAsync(ExecutionPlanId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        this.recurringJobManager.RemoveIfExists(id.ToString());

        return Task.CompletedTask;
    }

    private static Expression<Func<CreateReportJob, Task>> ExecuteReportJob(ReportScheduleId reportScheduleId)
    {
        return job => job.ExecuteAsync(null!, reportScheduleId.Value, CancellationToken.None);
    }
}
