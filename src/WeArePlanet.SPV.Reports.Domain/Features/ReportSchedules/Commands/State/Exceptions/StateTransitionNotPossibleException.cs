using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State.Exceptions;

public class StateTransitionNotPossibleException : DomainException
{
    public StateTransitionNotPossibleException(ReportSchedule schedule, IReportScheduleState desiredState)
        : base($"Cannot transition a schedule in state [{schedule.State}] to [{desiredState}].")
    {
    }

    public StateTransitionNotPossibleException(Exception exception)
        : base(exception)
    {
    }

    public StateTransitionNotPossibleException(ReportSchedule schedule, IReportScheduleState desiredState,
        Exception exception)
        : base($"Cannot transition a schedule in state [{schedule.State}] to [{desiredState}].", exception)
    {
    }
}
