using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

public class InactiveReportScheduleState : ValueObject<InactiveReportScheduleState>, IReportScheduleState
{
    public const string Alias = "Inactive";

    public string Name => Alias;

    public bool CanTransitionTo(IReportScheduleState desiredState)
    {
        return desiredState
            is ActiveReportScheduleState
            or ExecutedReportScheduleState
            or InactiveReportScheduleState
            or ArchivedReportScheduleState;
    }

    public object Clone()
    {
        return new InactiveReportScheduleState();
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
