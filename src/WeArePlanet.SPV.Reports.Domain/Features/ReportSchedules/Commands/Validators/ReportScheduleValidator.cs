using FluentValidation;
using FluentValidation.Validators;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

public class ReportScheduleValidator : AbstractValidator<ReportSchedule>
{
    public ReportScheduleValidator(
        IPropertyValidator<ReportSchedule, ReportScheduleInput?> inputValidator,
        IValidator<OneOffReportExecutionPlan> oneOffReportScheduleValidator,
        IValidator<RecurringReportExecutionPlan> recurringReportScheduleValidator)
    {
        this.RuleFor(schedule => schedule.Name).NotEmpty();
        this.RuleFor(schedule => schedule.Owner).NotNull();

        this.RuleFor(schedule => schedule.ExecutionPlan).NotNull()
            .SetInheritanceValidator(validator => validator
                .Add(oneOffReportScheduleValidator)
                .Add(recurringReportScheduleValidator));

        this.RuleFor(schedule => schedule.Template).NotNull()
            .WithMessage("A template for the provided identifier was not found.");

        this.RuleFor(schedule => schedule.WebhookConfiguration)
            .ChildRules(webhookConfigurationValidator =>
            {
                webhookConfigurationValidator.RuleFor(webhookConfiguration => webhookConfiguration!.OnSuccess)
                    .NotNull();
                webhookConfigurationValidator.RuleFor(webhookConfiguration => webhookConfiguration!.OnFailure)
                    .NotNull();
            })
            .When(schedule => schedule.WebhookConfiguration != null);

        this.RuleFor(configuration => configuration.Input)
            .SetValidator(inputValidator)
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            .When(configuration => configuration.Template != null)
            .WithMessage("Input object is invalid.");
    }
}
