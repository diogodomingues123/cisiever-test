namespace WeArePlanet.SPV.Reports.Domain.Common;

public sealed class TimeZoneId : ValueObject<TimeZoneId>
{
    public TimeZoneId(string value)
    {
        this.Value = value;
    }

    public string Value { get; }

    public static TimeZoneId Utc { get; } = new(TimeZoneInfo.Utc.Id);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }

    public static implicit operator TimeZoneInfo(TimeZoneId timeZoneId)
    {
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId.Value);
    }
}
