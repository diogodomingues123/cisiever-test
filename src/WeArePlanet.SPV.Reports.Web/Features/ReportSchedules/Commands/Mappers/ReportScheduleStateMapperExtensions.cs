using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

public static class ReportScheduleStateMapperExtensions
{
    public static ReportScheduleContractState? ToContractState(this IReportScheduleState state)
    {
        return state switch
        {
            ActiveReportScheduleState => ReportScheduleContractState.Active,
            InactiveReportScheduleState => ReportScheduleContractState.Inactive,
            ExecutedReportScheduleState => ReportScheduleContractState.Executed,
            _ => null
        };
    }
}
