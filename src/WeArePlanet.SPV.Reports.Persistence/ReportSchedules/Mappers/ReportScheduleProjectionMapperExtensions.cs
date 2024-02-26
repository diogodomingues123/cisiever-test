using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class ReportScheduleProjectionMapperExtensions
{
    public static ReportScheduleProjection? ToDomainProjection(this ReportScheduleDocument? reportSchedule,
        ReportTemplateConfiguration? template = default)
    {
        if (reportSchedule == null)
        {
            return null;
        }

        return new ReportScheduleProjection
        {
            Id = new ReportScheduleId(Guid.Parse(reportSchedule.Id)),
            Name = reportSchedule.Name,
            Owner = reportSchedule.Owner.ToDomainOwner()!,
            ExecutionPlan = reportSchedule.ExecutionPlan.ToDomainExecutionPlan()!,
            Template = template,
            CreatedAt = reportSchedule.CreatedAt,
            UpdatedAt = reportSchedule.UpdatedAt,
            Input = reportSchedule.Input != null
                ? new ReportScheduleInput(reportSchedule.Input)
                : null,
            WebhookConfiguration = reportSchedule.WebhookConfiguration.ToDomainWebhookConfiguration(),
            State = MapReportScheduleState(reportSchedule.State)
        };
    }

    private static IReportScheduleState MapReportScheduleState(string reportScheduleState)
    {
        return reportScheduleState switch
        {
            ActiveReportScheduleState.Alias => new ActiveReportScheduleState(),
            InactiveReportScheduleState.Alias => new InactiveReportScheduleState(),
            ExecutedReportScheduleState.Alias => new ExecutedReportScheduleState(),
            ArchivedReportScheduleState.Alias => new ArchivedReportScheduleState(),
            _ => throw new InvalidOperationException($"No state implementation matches [{reportScheduleState}].")
        };
    }
}
