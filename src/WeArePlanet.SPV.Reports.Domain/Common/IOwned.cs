using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Common;

public interface IOwned
{
    Owner Owner { get; }
}
