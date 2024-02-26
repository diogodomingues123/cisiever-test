using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

[ExcludeFromCodeCoverage]
public class TimeSpanFieldType : IFieldType
{
    public string Name => "TimeSpan";

    public object? ExtractFrom(ReportScheduleInput input, string name)
    {
        return input.GetParameter(name, obj => ExtractTimeSpan(obj));
    }

    private static TimeSpan? ExtractTimeSpan(object? arg)
    {
        if (arg == null)
        {
            return default;
        }

        return TimeSpan.TryParse(arg.ToString(), out var timeSpan)
            ? timeSpan
            : throw new InvalidOperationException("Parameter cannot be cast to type TimeSpan.");
    }
}
