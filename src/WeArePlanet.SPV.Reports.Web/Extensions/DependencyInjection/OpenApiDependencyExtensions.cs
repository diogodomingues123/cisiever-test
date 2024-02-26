using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class OpenApiDependencyExtensions
{
    public static IServiceCollection AddOpenApiServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddTypedOptions<DocumentationSettings>(
            configuration,
            SettingsConstants.DocumentationSectionKey);

        return serviceCollection;
    }

    public static IApplicationBuilder UseDocumentationServices(this IApplicationBuilder applicationBuilder)
    {
        var docSettings = applicationBuilder.ApplicationServices.GetRequiredService<IOptions<DocumentationSettings>>();

        if (docSettings.Value.Swagger.Enabled)
        {
            applicationBuilder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(docSettings.Value.OpenApiSpecUrl, "v1");
            });
        }

        if (docSettings.Value.RedocSettings.Enabled)
        {
            applicationBuilder.UseReDoc(options =>
            {
                options.DocumentTitle = "Reports API Documentation";
                options.SpecUrl = docSettings.Value.OpenApiSpecUrl;
                options.RoutePrefix = docSettings.Value.RedocSettings.RoutePrefix;
            });
        }

        return applicationBuilder;
    }
}
