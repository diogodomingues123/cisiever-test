using Cronos;

using FluentValidation;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

public class RecurringReportExecutionPlanValidator : AbstractValidator<RecurringReportExecutionPlan>
{
    public RecurringReportExecutionPlanValidator(IValidator<TimeZoneId> timeZoneValidator)
    {
        this.RuleFor(schedule => schedule.Frequency)
            .NotNull()
            .Must(frequency =>
            {
                try
                {
                    _ = CronExpression.Parse(frequency);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            })
            .WithMessage("Frequency is either missing or in an invalid format. It must be a valid CRON expression.");

        this.RuleFor(schedule => schedule.TimeZoneId)
            .SetValidator(timeZoneValidator);
    }
}
