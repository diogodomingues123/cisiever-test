using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Persistence.Settings;

[ExcludeFromCodeCoverage]
public class MongoSettings
{
    public required string DatabaseName { get; init; }

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}
