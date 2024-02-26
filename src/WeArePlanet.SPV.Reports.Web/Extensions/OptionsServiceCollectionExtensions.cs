using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

namespace WeArePlanet.SPV.Reports.Web.Extensions;

[ExcludeFromCodeCoverage]
internal static class OptionsServiceCollectionExtensions
{
    public static OptionsBuilder<T> AddTypedOptions<T>(this IServiceCollection serviceCollection,
        IConfiguration configuration, string sectionName)
        where T : class
    {
        var configurationSection = configuration.GetRequiredSection(sectionName);
        return serviceCollection.AddTypedOptions<T>(configurationSection);
    }

    public static IOptions<T> AddTypedOptionsAndGet<T>(this IServiceCollection serviceCollection,
        IConfiguration configuration, string sectionName)
        where T : class
    {
        var configurationSection = configuration.GetRequiredSection(sectionName);
        return serviceCollection.AddTypedOptionsAndGet<T>(configurationSection);
    }

    public static OptionsBuilder<T> AddTypedOptions<T>(this IServiceCollection serviceCollection,
        IConfigurationSection configurationSection)
        where T : class
    {
        return serviceCollection.AddOptions<T>()
            .Bind(configurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static IOptions<T> AddTypedOptionsAndGet<T>(this IServiceCollection serviceCollection,
        IConfigurationSection configurationSection)
        where T : class
    {
        serviceCollection.AddTypedOptions<T>(configurationSection);
        return Options.Create(configurationSection.Get<T>()!);
    }
}
