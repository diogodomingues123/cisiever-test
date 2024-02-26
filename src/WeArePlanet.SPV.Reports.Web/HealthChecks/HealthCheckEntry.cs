using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WeArePlanet.SPV.Reports.Web.HealthChecks;

[ExcludeFromCodeCoverage]
public class HealthCheckEntry
{
    public required string Name { get; set; }
    public required HealthStatus Status { get; set; }
    public TimeSpan? Duration { get; set; }
    public IReadOnlyCollection<string>? Tags { get; set; }
    public IReadOnlyDictionary<string, object>? Data { get; set; }
}
