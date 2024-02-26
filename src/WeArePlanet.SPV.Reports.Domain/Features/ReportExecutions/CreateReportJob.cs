using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Hangfire;
using Hangfire.Server;

using WeArePlanet.SPV.Reports.Common.Hangfire;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportExecutions;

// TODO remove code coverage exclusion attribute in SIP-4073
[ExcludeFromCodeCoverage]
public class CreateReportJob
{
    private readonly IDateTimeProvider provider;
    private readonly IReportScheduleRepository repository;

    public CreateReportJob(IDateTimeProvider provider, IReportScheduleRepository repository)
    {
        this.provider = provider;
        this.repository = repository;
    }

    [JobDisplayName("Generate Report - Schedule [{1}]")]
    [InjectParametersJobFilter]
    public async Task ExecuteAsync(PerformContext context,
        [JobParameterName("Report Schedule Id")]
        Guid reportScheduleId, CancellationToken cancellationToken)
    {
        var id = new ReportScheduleId(reportScheduleId);
        var scheduleProjection = await this.repository.GetByIdAsync(id, cancellationToken);

        if (scheduleProjection == null)
        {
            // TODO: What if it does not exist anymore (maybe log)?
            return;
        }

        var schedule = scheduleProjection.ToReportSchedule();

        if (schedule.HasExecuted())
        {
            // TODO: What if it not executable?
            return;
        }

        // Simulate work
        Debug.WriteLine(this.provider.GetUtcNow());
        Console.WriteLine(this.provider.GetUtcNow());

        schedule.MarkAsExecuted(this.provider);
        await this.repository.AddOrUpdateAsync(schedule, cancellationToken);
    }
}
