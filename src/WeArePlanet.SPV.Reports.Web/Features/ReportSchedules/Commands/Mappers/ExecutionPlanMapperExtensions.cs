using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

public static class ExecutionPlanMapperExtensions
{
    public static IReportExecutionPlan? ToDomainReportExecutionPlan(this ExecutionPlanContract executionPlan)
    {
        return executionPlan switch
        {
            OneOffExecutionPlanContract oneOffContractExecutionPlan => new OneOffReportExecutionPlan(
                oneOffContractExecutionPlan.Date,
                oneOffContractExecutionPlan.TimeZone == null
                    ? null
                    : new TimeZoneId(oneOffContractExecutionPlan.TimeZone)),
            RecurringExecutionPlanContract recurringContractExecutionPlan => new RecurringReportExecutionPlan(
                recurringContractExecutionPlan.Frequency,
                recurringContractExecutionPlan.TimeZone == null
                    ? null
                    : new TimeZoneId(recurringContractExecutionPlan.TimeZone)),
            _ => null
        };
    }

    public static ExecutionPlanContract? ToContractExecutionPlan(this IReportExecutionPlan schedule,
        IDateTimeProvider dateTimeProvider)
    {
        return schedule switch
        {
            OneOffReportExecutionPlan oneOffDomainSchedule => new OneOffExecutionPlanContract(
                oneOffDomainSchedule.Date,
                schedule.Id?.Value,
                schedule.GetNextExecutionDateInUtc(dateTimeProvider),
                oneOffDomainSchedule.TimeZoneId?.Value),
            RecurringReportExecutionPlan recurringDomainSchedule => new RecurringExecutionPlanContract(
                recurringDomainSchedule.Frequency,
                schedule.Id?.Value,
                schedule.GetNextExecutionDateInUtc(dateTimeProvider),
                recurringDomainSchedule.TimeZoneId?.Value),
            _ => null
        };
    }
}
