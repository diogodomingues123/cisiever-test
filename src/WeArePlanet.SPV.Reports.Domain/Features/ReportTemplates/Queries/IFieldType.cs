using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

public interface IFieldType
{
    string Name { get; }
    object? ExtractFrom(ReportScheduleInput input, string name);
}
