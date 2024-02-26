using System.Diagnostics.CodeAnalysis;

using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.CosmosDB;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class JobDependencyExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection serviceCollection, IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        serviceCollection.AddTypedOptions<HangfireSettings>(configuration, SettingsConstants.HangfireSectionKey);

        return serviceCollection.AddHangfire((serviceProvider, options) =>
            {
                var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
                var mongoUrl = serviceProvider.GetRequiredService<MongoUrl>();
                var hangfireSettings = serviceProvider.GetRequiredService<IOptions<HangfireSettings>>();

                options
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseRecommendedSerializerSettings()
                    .UseCosmosStorage(mongoClient, hangfireSettings.Value.DatabaseName,
                        new CosmosStorageOptions
                        {
                            CheckConnection = true,
                            ConnectionCheckTimeout = hangfireSettings.Value.DatabaseTimeout,
                            QueuePollInterval = hangfireSettings.Value.QueuePollInterval,
                            CheckQueuedJobsStrategy =
                                string.IsNullOrWhiteSpace(mongoUrl.ReplicaSetName)
                                    ? CheckQueuedJobsStrategy.Poll
                                    : CheckQueuedJobsStrategy.Watch,
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new NoneMongoBackupStrategy()
                            },
                            SupportsCappedCollection = hangfireSettings.Value.SupportsCappedCollection
                        });
            })
            .AddHangfireServer((serviceProvider, options) =>
            {
                var hangfireSettings = serviceProvider.GetRequiredService<IOptions<HangfireSettings>>();

                options.CancellationCheckInterval = hangfireSettings.Value.CancellationCheckInterval;
            });
    }
}
