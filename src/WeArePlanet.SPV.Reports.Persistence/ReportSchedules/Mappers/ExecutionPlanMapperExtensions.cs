using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class ExecutionPlanMapperExtensions
{
    public static ExecutionPlanDocument ToMongoDocument(this IReportExecutionPlan schedule)
    {
        if (schedule == null)
        {
            throw new ArgumentNullException(nameof(schedule));
        }

        return schedule switch
        {
            OneOffReportExecutionPlan oneOff => new ExecutionPlanDocument
            {
                Id = oneOff.Id?.Value!,
                Type = ScheduleDocumentType.OneOff,
                Date = oneOff.Date,
                TimeZoneId = oneOff.TimeZoneId.Value
            },
            RecurringReportExecutionPlan recurring => new ExecutionPlanDocument
            {
                Id = recurring.Id?.Value!,
                Type = ScheduleDocumentType.Recurring,
                Frequency = recurring.Frequency,
                TimeZoneId = recurring.TimeZoneId.Value
            },
            _ => throw new InvalidOperationException(
                $"Execution Plan type {schedule.GetType().Name} is not mapped to a mongo document type.")
        };
    }

    public static IReportExecutionPlan? ToDomainExecutionPlan(this ExecutionPlanDocument? document)
    {
        if (document == null)
        {
            return null;
        }

        return document.Type switch
        {
            ScheduleDocumentType.OneOff => new OneOffReportExecutionPlan(
                new ExecutionPlanId(document.Id),
                document.Date?.UtcDateTime,
                document.TimeZoneId?.ToDomainTimeZoneId()),
            ScheduleDocumentType.Recurring => new RecurringReportExecutionPlan(
                new ExecutionPlanId(document.Id),
                document.Frequency!,
                document.TimeZoneId?.ToDomainTimeZoneId()),
            _ => throw new InvalidOperationException(
                $"Execution Plan type {document.Type} is not mapped to a domain type.")
        };
    }
}
