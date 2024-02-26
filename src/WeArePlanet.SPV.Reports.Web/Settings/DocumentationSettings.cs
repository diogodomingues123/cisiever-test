using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Settings;

[ExcludeFromCodeCoverage]
public class DocumentationSettings
{
    public string OpenApiSpecUrl { get; init; } = string.Empty;

    public SwaggerSettings Swagger { get; init; } = new();

    public RedocSettings RedocSettings { get; init; } = new();
}

[ExcludeFromCodeCoverage]
public class RedocSettings
{
    public bool Enabled { get; init; } = true;

    public string RoutePrefix { get; init; } = "docs";
}

[ExcludeFromCodeCoverage]
public class SwaggerSettings
{
    public bool Enabled { get; init; } = false;
}
