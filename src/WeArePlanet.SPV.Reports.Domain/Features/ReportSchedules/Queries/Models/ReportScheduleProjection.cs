using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;

public class ReportScheduleProjection : IOwned
{
    public required ReportScheduleId Id { get; init; }

    public required string Name { get; init; }

    public required ReportTemplateConfiguration Template { get; init; }

    public required IReportExecutionPlan ExecutionPlan { get; init; }

    public ReportScheduleInput? Input { get; init; }

    public WebhookConfiguration? WebhookConfiguration { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public required IReportScheduleState State { get; init; }

    public required Owner Owner { get; init; }

    public ReportSchedule ToReportSchedule()
    {
        return new ReportSchedule(
            this.Id, this.Name, this.Owner, this.Template, this.ExecutionPlan, this.State)
        {
            WebhookConfiguration = this.WebhookConfiguration,
            Input = this.Input,
            CreatedAt = this.CreatedAt,
            UpdatedAt = this.UpdatedAt
        };
    }
}
