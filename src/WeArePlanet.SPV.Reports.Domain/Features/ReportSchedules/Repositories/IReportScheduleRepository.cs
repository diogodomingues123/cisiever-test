using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

public interface IReportScheduleRepository
{
    Task AddOrUpdateAsync(ReportSchedule reportSchedule, CancellationToken cancellationToken = default);

    Task<ReportScheduleProjection?> GetByIdAsync(ReportScheduleId requestId,
        CancellationToken cancellationToken = default);

    Task<ReportScheduleProjection?> MarkAsExecuted(ReportScheduleId reportScheduleId,
        CancellationToken cancellationToken = default);

    Task<Page<ReportScheduleProjection>> GetAllAsync(GetReportSchedulesQuery query, IAuthenticationPrincipal principal,
        CancellationToken cancellationToken = default);
}
