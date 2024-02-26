namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

public class ArchivedReportScheduleState : IReportScheduleState
{
    public const string Alias = "Archived";

    public string Name => Alias;

    public bool CanTransitionTo(IReportScheduleState desiredState)
    {
        return desiredState is ArchivedReportScheduleState;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public object Clone()
    {
        return new ArchivedReportScheduleState();
    }
}
