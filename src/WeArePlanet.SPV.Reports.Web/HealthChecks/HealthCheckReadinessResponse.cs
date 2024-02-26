using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WeArePlanet.SPV.Reports.Web.HealthChecks;

[ExcludeFromCodeCoverage]
public class HealthCheckReadinessResponse
{
    public required HealthStatus Status { get; init; }
    public required IReadOnlyCollection<HealthCheckEntry> Checks { get; init; }
}
