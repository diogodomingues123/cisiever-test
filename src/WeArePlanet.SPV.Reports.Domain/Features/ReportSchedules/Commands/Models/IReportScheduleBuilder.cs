using FluentValidation.Results;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public interface IReportScheduleBuilder
{
    IReportScheduleBuilder FromExisting(ReportScheduleProjection projection);
    IReportScheduleBuilder UsingTemplateConfiguration(Guid? templateConfigurationId);
    IReportScheduleBuilder WithName(string? name);
    IReportScheduleBuilder WithExecutionPlan(IReportExecutionPlan? reportSchedule);
    IReportScheduleBuilder WithInput(ReportScheduleInput? input);
    IReportScheduleBuilder WithOwner(Owner? owner);
    IReportScheduleBuilder WithWebhooks(WebhookConfiguration? webhookConfiguration);
    IReportScheduleBuilder IsActive(bool? toggle);

    Task<Either<ValidationResult, ReportSchedule>> BuildAsync(CancellationToken cancellationToken = default);
}
