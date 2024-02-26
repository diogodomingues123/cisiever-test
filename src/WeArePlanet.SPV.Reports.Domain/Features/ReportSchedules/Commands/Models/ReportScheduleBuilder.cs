using FluentValidation;
using FluentValidation.Results;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class ReportScheduleBuilder : IReportScheduleBuilder
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IReportTemplateConfigurationsRepository templateConfigurationsRepository;
    private readonly IValidator<ReportSchedule> validator;
    private IReportExecutionPlan? executionPlan;

    private ReportSchedule? existing;

    private ReportScheduleInput? input;
    private bool? isActiveToSet;
    private string? name;
    private Owner? owner;
    private Guid? templateConfigurationId;
    private WebhookConfiguration? webhookConfiguration;

    public ReportScheduleBuilder(
        IValidator<ReportSchedule> validator,
        IReportTemplateConfigurationsRepository templateConfigurationsRepository,
        IDateTimeProvider dateTimeProvider)
    {
        this.validator = validator;
        this.templateConfigurationsRepository = templateConfigurationsRepository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public IReportScheduleBuilder FromExisting(ReportScheduleProjection projection)
    {
        this.existing = projection.ToReportSchedule();

        return this;
    }

    public IReportScheduleBuilder WithName(string? name)
    {
        this.name = name;
        return this;
    }

    public IReportScheduleBuilder WithExecutionPlan(IReportExecutionPlan? reportSchedule)
    {
        this.executionPlan = reportSchedule;
        return this;
    }

    public IReportScheduleBuilder WithInput(ReportScheduleInput? input)
    {
        this.input = input;
        return this;
    }

    public IReportScheduleBuilder WithOwner(Owner? owner)
    {
        this.owner = owner;
        return this;
    }

    public IReportScheduleBuilder WithWebhooks(WebhookConfiguration? webhookConfiguration)
    {
        this.webhookConfiguration = webhookConfiguration;
        return this;
    }

    public IReportScheduleBuilder UsingTemplateConfiguration(Guid? templateConfigurationId)
    {
        this.templateConfigurationId = templateConfigurationId;
        return this;
    }

    public IReportScheduleBuilder IsActive(bool? toggle)
    {
        this.isActiveToSet = toggle;
        return this;
    }

    public async Task<Either<ValidationResult, ReportSchedule>> BuildAsync(
        CancellationToken cancellationToken = default)
    {
        var templateConfiguration = this.templateConfigurationId.HasValue
            ? await this.FetchTemplateConfigurationAsync(this.templateConfigurationId.Value, cancellationToken)
            : this.existing?.Template;

        var idToSet = this.existing?.Id ?? ReportScheduleId.Create();
        var utcNow = this.dateTimeProvider.GetUtcNow().UtcDateTime;

        var reportSchedule = new ReportSchedule(
            idToSet,
            this.name ?? this.existing?.Name!,
            this.existing?.Owner ?? this.owner!,
            templateConfiguration!,
            this.executionPlan ?? this.existing?.ExecutionPlan!,
            this.existing?.State ?? new InactiveReportScheduleState())
        {
            WebhookConfiguration = this.webhookConfiguration ?? this.existing?.WebhookConfiguration,
            Input = this.input ?? this.existing?.Input,
            CreatedAt = this.existing?.CreatedAt ?? utcNow,
            UpdatedAt = utcNow
        };

        if (this.isActiveToSet.HasValue)
        {
            try
            {
                reportSchedule.ToggleActivation(this.isActiveToSet.Value, this.dateTimeProvider);
            }
            catch (StateTransitionNotPossibleException ex)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(ReportSchedule.State), ex.Message) });
            }
        }

        var validationResult = await this.validator.ValidateAsync(reportSchedule, cancellationToken);

        return validationResult.IsValid ? reportSchedule : validationResult;
    }

    private async Task<ReportTemplateConfiguration?> FetchTemplateConfigurationAsync(Guid safeTemplateConfigurationId,
        CancellationToken cancellationToken = default)
    {
        var result = await this.templateConfigurationsRepository.GetAsync(safeTemplateConfigurationId,
            cancellationToken);

        return result.Match(configuration => configuration, _ => null!);
    }
}
