namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

public interface IReportScheduleState : ICloneable
{
    string Name { get; }

    bool CanTransitionTo(IReportScheduleState desiredState);

    string? ToString();
}
