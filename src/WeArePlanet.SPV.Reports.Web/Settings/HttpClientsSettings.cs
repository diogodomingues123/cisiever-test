using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Settings;

[ExcludeFromCodeCoverage]
public class HttpClientsSettings
{
    [Required]
    public required HttpClientSettings Webhooks { get; init; } = new();
}

[ExcludeFromCodeCoverage]
public class HttpClientSettings
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(1);
}
