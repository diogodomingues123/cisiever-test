using MediatR;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Handlers;

public class ArchiveReportScheduleCommandHandler : ICommandHandler<ArchiveReportScheduleCommand, Unit>
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMediator mediator;
    private readonly IReportScheduleRepository repository;

    public ArchiveReportScheduleCommandHandler(IMediator mediator, IReportScheduleRepository repository,
        IDateTimeProvider dateTimeProvider)
    {
        this.mediator = mediator;
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(ArchiveReportScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportScheduleProjection =
                await this.mediator.Send(new GetReportScheduleByIdQuery(request.Id), cancellationToken);

            var reportSchedule = reportScheduleProjection.ToReportSchedule();

            reportSchedule.Archive(this.dateTimeProvider);

            await this.repository.AddOrUpdateAsync(reportSchedule, cancellationToken);
        }
        catch (NotFoundException)
        {
            // Don't do anything
        }

        return Unit.Value;
    }
}
