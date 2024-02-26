using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

[ExcludeFromCodeCoverage]
public class EnumFieldType<T> : IFieldType
    where T : struct, Enum
{
    public string Name => typeof(T).Name;

    public object? ExtractFrom(ReportScheduleInput input, string name)
    {
        return ExtractEnum(input.GetParameter<string>(name));
    }

    private static T? ExtractEnum(string? arg)
    {
        if (arg == null)
        {
            return default;
        }

        return Enum.TryParse<T>(arg, out var enumValue) && Enum.IsDefined(enumValue)
            ? enumValue
            : throw new InvalidOperationException($"Cannot convert value {arg} to a valid {typeof(T).Name}");
    }
}
