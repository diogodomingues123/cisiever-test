using FluentAssertions.Extensions;

using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.Common;

public sealed class StaticDateTimeProvider : IDateTimeProvider
{
    public static readonly DateTimeOffset CurrentDateTimeOffset =
        1.February(2023).At(16.Hours().And(5.Minutes())).WithOffset(TimeSpan.Zero);

    public static readonly StaticDateTimeProvider Instance = new();

    private StaticDateTimeProvider()
    {
    }

    public DateTimeOffset GetUtcNow()
    {
        return CurrentDateTimeOffset;
    }

    public DateTime ConvertToUtc(DateTime date, TimeZoneId sourceTimeZoneId)
    {
        return TimeZoneInfo.ConvertTimeToUtc(date, sourceTimeZoneId);
    }
}
