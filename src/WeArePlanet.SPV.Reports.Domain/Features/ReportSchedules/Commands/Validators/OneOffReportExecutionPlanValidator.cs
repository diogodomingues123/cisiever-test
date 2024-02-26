using FluentValidation;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

public class OneOffReportExecutionPlanValidator : AbstractValidator<OneOffReportExecutionPlan>
{
    public OneOffReportExecutionPlanValidator(IDateTimeProvider dateTimeProvider,
        IValidator<TimeZoneId> timeZoneValidator)
    {
        this.RuleFor(schedule => schedule.Date)
            .NotEqual(DateTime.MinValue)
            .Must(date => date > dateTimeProvider.GetUtcNow())
            .WithMessage("The schedule date must be in the future.")
            .When(schedule => schedule.Date.HasValue);

        this.RuleFor(schedule => schedule.TimeZoneId)
            .SetValidator(timeZoneValidator);
    }
}
