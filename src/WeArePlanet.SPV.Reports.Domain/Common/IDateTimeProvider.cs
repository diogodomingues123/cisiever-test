namespace WeArePlanet.SPV.Reports.Domain.Common;

public interface IDateTimeProvider
{
    DateTimeOffset GetUtcNow();

    DateTime ConvertToUtc(DateTime date, TimeZoneId sourceTimeZoneId);
}
