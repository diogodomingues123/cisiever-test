using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class TimeZoneMapperExtensions
{
    public static TimeZoneId ToDomainTimeZoneId(this string timeZone)
    {
        return new TimeZoneId(timeZone);
    }
}
