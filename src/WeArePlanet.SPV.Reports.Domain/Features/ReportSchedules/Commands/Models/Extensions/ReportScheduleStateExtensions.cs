using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models.Extensions;

internal static class ReportScheduleStateExtensions
{
    public static bool IsActive(this IReportScheduleState state)
    {
        return state is ActiveReportScheduleState;
    }
}
