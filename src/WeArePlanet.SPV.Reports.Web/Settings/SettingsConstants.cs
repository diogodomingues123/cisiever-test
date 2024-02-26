using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Settings;

[ExcludeFromCodeCoverage]
internal static class SettingsConstants
{
    public const string HttpClientsSectionKey = "HttpClients";
    public const string AuthSectionKey = "Auth";
    public const string HangfireSectionKey = "Hangfire";
    public const string MongoSectionKey = "Mongo";

    public const string MongoConnectionStringKey = "Mongo";
    public const string PaginationSectionKey = "Pagination";
    public const string DocumentationSectionKey = "Documentation";
}
