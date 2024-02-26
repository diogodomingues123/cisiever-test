using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class HttpClientsDependencyExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddTypedOptions<HttpClientsSettings>(configuration,
            SettingsConstants.HttpClientsSectionKey);

        serviceCollection.AddHttpClient("Webhooks", (provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<HttpClientsSettings>>().Value;

            client.Timeout = settings.Webhooks.Timeout;
        });

        return serviceCollection;
    }
}
