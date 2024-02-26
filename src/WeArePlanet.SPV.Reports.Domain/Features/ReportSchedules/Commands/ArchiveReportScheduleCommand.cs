using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands;

public class ArchiveReportScheduleCommand : ICommand
{
    public ArchiveReportScheduleCommand(ReportScheduleId id)
    {
        this.Id = id;
    }

    public ReportScheduleId Id { get; }
}
