using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportExecutions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ReportExecutionsFeatureExtensions
{
    public static IServiceCollection AddReportExecutionsFeature(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}
