using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

[ExcludeFromCodeCoverage]
public class StringFieldType : IFieldType
{
    public string Name => "String";

    public object? ExtractFrom(ReportScheduleInput input, string name)
    {
        return input.GetParameter<string?>(name);
    }
}
