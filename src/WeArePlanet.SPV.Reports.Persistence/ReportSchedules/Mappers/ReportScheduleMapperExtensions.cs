using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class ReportScheduleMapperExtensions
{
    public static ReportScheduleDocument ToMongoDocument(this ReportSchedule reportSchedule)
    {
        if (reportSchedule == null)
        {
            throw new ArgumentNullException(nameof(reportSchedule));
        }

        return new ReportScheduleDocument
        {
            Id = reportSchedule.Id!.Value.ToString()!,
            Name = reportSchedule.Name,
            Owner = reportSchedule.Owner.ToMongoDocument(),
            ExecutionPlan = reportSchedule.ExecutionPlan.ToMongoDocument(),
            TemplateId = reportSchedule.Template.Id.ToString(),
            CreatedAt = reportSchedule.CreatedAt,
            UpdatedAt = reportSchedule.UpdatedAt,
            Input = reportSchedule.Input != null
                ? new Dictionary<string, object>(reportSchedule.Input.Parameters)
                : null,
            WebhookConfiguration = reportSchedule.WebhookConfiguration.ToMongoDocument(),
            State = reportSchedule.State.ToString()!
        };
    }
}
