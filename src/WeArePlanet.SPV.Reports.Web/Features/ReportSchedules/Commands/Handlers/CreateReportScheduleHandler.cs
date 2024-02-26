using FluentValidation.Results;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Common.Auth.Extensions;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers;

public class CreateReportScheduleHandler : ICommandHandler<CreateReportScheduleRequestContract,
    Either<ValidationResult, ReportScheduleContract>>
{
    private readonly IReportScheduleBuilder builder;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IAuthenticationPrincipal principal;
    private readonly IReportScheduleRepository repository;

    public CreateReportScheduleHandler(
        IReportScheduleBuilder builder,
        IAuthenticationPrincipal principal,
        IReportScheduleRepository repository,
        IDateTimeProvider dateTimeProvider)
    {
        this.builder = builder;
        this.principal = principal;
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Either<ValidationResult, ReportScheduleContract>> Handle(
        CreateReportScheduleRequestContract scheduleRequest,
        CancellationToken cancellationToken)
    {
        this.builder
            .WithName(scheduleRequest.Name)
            .UsingTemplateConfiguration(scheduleRequest.TemplateId)
            .WithExecutionPlan(scheduleRequest.ExecutionPlan.ToDomainReportExecutionPlan())
            .WithOwner(this.principal.ToOwner())
            .WithInput(scheduleRequest.Input.ToDomainReportScheduleInput())
            .WithWebhooks(scheduleRequest.Webhooks.ToDomainWebhookConfiguration());

        var reportConfigurationCreationResult = await ReportSchedule.CreateAsync(this.builder, this.dateTimeProvider);

        return await reportConfigurationCreationResult.MapAsync(async configuration =>
        {
            await this.repository.AddOrUpdateAsync(configuration, cancellationToken);
            return configuration.ToContractReportScheduleResponse(this.dateTimeProvider);
        });
    }
}
