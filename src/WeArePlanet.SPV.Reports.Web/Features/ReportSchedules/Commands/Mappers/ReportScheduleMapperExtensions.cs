using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

public static class ReportScheduleMapperExtensions
{
    public static ReportScheduleContract ToContractReportScheduleResponse(
        this ReportSchedule reportSchedule,
        IDateTimeProvider dateTimeProvider)
    {
        return new ReportScheduleContract(
            id: reportSchedule.Id!.Value,
            name: reportSchedule.Name,
            executionPlan: reportSchedule.ExecutionPlan.ToContractExecutionPlan(dateTimeProvider),
            templateId: reportSchedule.Template.Id,
            input: reportSchedule.Input?.ToContractReportConfigurationInput(),
            webhooks: reportSchedule.WebhookConfiguration?.ToContractWebhooks(),
            createdAt: reportSchedule.CreatedAt,
            updatedAt: reportSchedule.UpdatedAt,
            state: reportSchedule.State.ToContractState());
    }
}
