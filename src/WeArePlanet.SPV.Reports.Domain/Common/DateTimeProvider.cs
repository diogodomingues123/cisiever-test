using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common;

[ExcludeFromCodeCoverage]
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset GetUtcNow()
    {
        return DateTimeOffset.UtcNow;
    }

    public DateTime ConvertToUtc(DateTime date, TimeZoneId sourceTimeZoneId)
    {
        if (date.Kind == DateTimeKind.Utc)
        {
            return date;
        }

        var sourceTzInfo = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId.Value);

        return TimeZoneInfo.ConvertTimeToUtc(date, sourceTzInfo);
    }
}
