using FluentValidation.Results;

using LanguageExt;

using MediatR;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Queries.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers;

public class PatchReportScheduleHandler : ICommandHandler<PatchReportScheduleRequest,
    Either<ValidationResult, ReportScheduleContract>>
{
    private readonly IReportScheduleBuilder builder;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMediator mediator;
    private readonly IReportScheduleRepository repository;

    public PatchReportScheduleHandler(
        IReportScheduleBuilder builder,
        IReportScheduleRepository repository,
        IDateTimeProvider dateTimeProvider,
        IMediator mediator)
    {
        this.builder = builder;
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
        this.mediator = mediator;
    }

    public async Task<Either<ValidationResult, ReportScheduleContract>> Handle(PatchReportScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var existingReportScheduleProjection =
            await this.mediator.Send(new GetReportScheduleByIdQuery(request.Id!), cancellationToken);

        if (request.IsEmpty())
        {
            // If the request does not cause any changes, then just return the instance directly.
            return existingReportScheduleProjection.ToReportScheduleContract(this.dateTimeProvider);
        }

        this.builder
            .FromExisting(existingReportScheduleProjection)
            .IsActive(request.Active);

        var updatedReportScheduleResult = await ReportSchedule.UpdateAsync(this.builder, this.dateTimeProvider);

        return await updatedReportScheduleResult.MapAsync(async configuration =>
        {
            await this.repository.AddOrUpdateAsync(configuration, cancellationToken);
            return configuration.ToContractReportScheduleResponse(this.dateTimeProvider);
        });
    }
}
