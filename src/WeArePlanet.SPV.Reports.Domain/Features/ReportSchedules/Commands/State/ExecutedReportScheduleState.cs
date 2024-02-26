using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

public class ExecutedReportScheduleState : ValueObject<ExecutedReportScheduleState>, IReportScheduleState
{
    public const string Alias = "Executed";

    public string Name => Alias;

    public bool CanTransitionTo(IReportScheduleState desiredState)
    {
        return desiredState
            is ExecutedReportScheduleState
            or ArchivedReportScheduleState;
    }

    public object Clone()
    {
        return new ExecutedReportScheduleState();
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
