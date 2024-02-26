using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Queries.Mappers;

public static class ReportScheduleProjectionPageExtensions
{
    public static ReportSchedulePageContract ToContractPage(this Page<ReportScheduleProjection> page,
        IDateTimeProvider dateTimeProvider)
    {
        var entries = page.Entries
            .Select(entry => entry.ToReportScheduleContract(dateTimeProvider))
            .ToList();

        return new ReportSchedulePageContract(
            entries,
            page.PageNumber,
            (int)page.TotalItems,
            page.TotalPages
        );
    }
}
