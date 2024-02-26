using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Settings;

[ExcludeFromCodeCoverage]
public class HangfireSettings
{
    [Required]
    public required string DatabaseName { get; init; } = "hangfire";

    public TimeSpan QueuePollInterval { get; init; } = TimeSpan.FromSeconds(10);

    public TimeSpan CancellationCheckInterval { get; init; } = TimeSpan.FromSeconds(5);

    public TimeSpan DatabaseTimeout { get; init; } = TimeSpan.FromSeconds(30);

    public bool SupportsCappedCollection { get; init; } = false;

    public bool DisplayStorageConnectionString { get; init; } = false;

    public HangfireDashboardSettings Dashboard { get; init; } = new();
}

[ExcludeFromCodeCoverage]
public class HangfireDashboardSettings
{
    public bool Enabled { get; init; } = false;

    public bool RequireAuthentication { get; init; } = true;
}
