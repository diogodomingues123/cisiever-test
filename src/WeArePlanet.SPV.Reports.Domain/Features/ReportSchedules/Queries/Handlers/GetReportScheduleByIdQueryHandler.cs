using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Handlers;

public class GetReportScheduleByIdQueryHandler : IQueryHandler<GetReportScheduleByIdQuery, ReportScheduleProjection>
{
    private readonly IReportScheduleRepository repository;

    public GetReportScheduleByIdQueryHandler(IReportScheduleRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ReportScheduleProjection> Handle(GetReportScheduleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var reportScheduleProjection = await this.repository.GetByIdAsync(query.Id, cancellationToken);

        if (reportScheduleProjection == null)
        {
            throw new NotFoundException(query.Id);
        }

        return reportScheduleProjection;
    }
}
