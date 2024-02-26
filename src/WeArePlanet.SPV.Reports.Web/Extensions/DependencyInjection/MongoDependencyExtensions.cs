using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

using WeArePlanet.SPV.Reports.Persistence.Settings;
using WeArePlanet.SPV.Reports.Web.HealthChecks;
using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class MongoDependencyExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection serviceCollection, IConfiguration configuration,
        IHealthChecksBuilder healthCheckBuilder)
    {
        var mongoConnectionString = configuration.GetConnectionString(SettingsConstants.MongoConnectionStringKey);

        if (mongoConnectionString == null)
        {
            throw new InvalidOperationException("Mongo Connection String cannot be empty.");
        }

        var options =
            serviceCollection.AddTypedOptionsAndGet<MongoSettings>(configuration, SettingsConstants.MongoSectionKey);

        ApplyConventions();

        var mongoUrl = new MongoUrl(mongoConnectionString);

        healthCheckBuilder.AddMongoDb(
            mongoConnectionString,
            options.Value.DatabaseName,
            "Reports Database (MongoDB)",
            tags: new[] { HealthCheckConstants.OnReadinessTag });

        return serviceCollection
            .AddSingleton(mongoUrl)
            .AddSingleton<IMongoClient>(provider =>
            {
                var mongoSettings = provider.GetRequiredService<IOptions<MongoSettings>>();
                var settings = MongoClientSettings.FromUrl(provider.GetRequiredService<MongoUrl>());
                settings.ConnectTimeout = mongoSettings.Value.Timeout;

                return new MongoClient(settings);
            });
    }

    private static void ApplyConventions()
    {
        ConventionRegistry.Register(
            "IgnoreIfNullConvention",
            new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
    }
}
