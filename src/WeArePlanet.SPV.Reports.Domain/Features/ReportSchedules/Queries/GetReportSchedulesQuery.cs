using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;

public class GetReportSchedulesQuery : ValueObject<GetReportSchedulesQuery>, IQuery<Page<ReportScheduleProjection>>
{
    public GetReportSchedulesQuery(int? pageNumber, int? pageSize)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }

    public int? PageNumber { get; }

    public int? PageSize { get; }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.PageNumber;
        yield return this.PageSize;
    }
}
