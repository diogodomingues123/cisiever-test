using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;
using WeArePlanet.SPV.Reports.Persistence.ReportTemplates;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportTemplates.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ReportTemplatesFeatureExtensions
{
    public static IServiceCollection AddReportTemplatesFeature(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IReportTemplateConfigurationsRepository, ReportTemplateConfigurationsRepository>();
    }
}
