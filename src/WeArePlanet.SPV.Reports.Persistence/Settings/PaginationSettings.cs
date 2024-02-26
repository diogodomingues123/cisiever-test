using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Persistence.Settings;

[ExcludeFromCodeCoverage]
public class PaginationSettings
{
    public int DefaultPageSize { get; set; } = 25;
}
