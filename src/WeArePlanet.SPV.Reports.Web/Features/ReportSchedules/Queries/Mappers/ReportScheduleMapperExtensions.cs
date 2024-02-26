using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Queries.Mappers;

public static class ReportScheduleProjectionMapperExtensions
{
    public static ReportScheduleContract ToReportScheduleContract(
        this ReportScheduleProjection reportScheduleProjection, IDateTimeProvider dateTimeProvider)
    {
        return new ReportScheduleContract(
            id: reportScheduleProjection.Id!.Value,
            name: reportScheduleProjection.Name,
            executionPlan: reportScheduleProjection.ExecutionPlan.ToContractExecutionPlan(dateTimeProvider),
            templateId: reportScheduleProjection.Template.Id,
            input: reportScheduleProjection.Input?.ToContractReportConfigurationInput(),
            webhooks: reportScheduleProjection.WebhookConfiguration?.ToContractWebhooks(),
            createdAt: reportScheduleProjection.CreatedAt,
            updatedAt: reportScheduleProjection.UpdatedAt,
            state: reportScheduleProjection.State.ToContractState());
    }
}
