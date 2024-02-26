using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Handlers;

public class GetReportSchedulesQueryHandler : IQueryHandler<GetReportSchedulesQuery, Page<ReportScheduleProjection>>
{
    private readonly IAuthenticationPrincipal principal;
    private readonly IReportScheduleRepository repository;

    public GetReportSchedulesQueryHandler(IReportScheduleRepository repository, IAuthenticationPrincipal principal)
    {
        this.repository = repository;
        this.principal = principal;
    }

    public async Task<Page<ReportScheduleProjection>> Handle(
        GetReportSchedulesQuery query,
        CancellationToken cancellationToken)
    {
        return await this.repository.GetAllAsync(query, this.principal, cancellationToken);
    }
}
