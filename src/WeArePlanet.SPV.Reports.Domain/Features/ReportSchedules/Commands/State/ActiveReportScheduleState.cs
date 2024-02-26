using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

public class ActiveReportScheduleState : ValueObject<ActiveReportScheduleState>, IReportScheduleState
{
    public const string Alias = "Active";

    public string Name => Alias;

    public bool CanTransitionTo(IReportScheduleState desiredState)
    {
        return desiredState
            is ExecutedReportScheduleState
            or InactiveReportScheduleState
            or ActiveReportScheduleState
            or ArchivedReportScheduleState;
    }

    public object Clone()
    {
        return new ActiveReportScheduleState();
    }

    public override string ToString()
    {
        return this.Name;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Name;
    }
}
