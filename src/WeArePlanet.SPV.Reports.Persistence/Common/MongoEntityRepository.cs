using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Persistence.Common.Extensions;
using WeArePlanet.SPV.Reports.Persistence.Events;
using WeArePlanet.SPV.Reports.Persistence.Settings;

namespace WeArePlanet.SPV.Reports.Persistence.Common;

[ExcludeFromCodeCoverage]
public abstract class MongoEntityRepository
{
    private readonly InsertManyOptions insertManyOptions;
    private readonly IMediator mediator;
    protected readonly IMongoClient mongoClient;
    protected readonly IOptions<MongoSettings> mongoSettings;

    protected MongoEntityRepository(IOptions<MongoSettings> mongoSettings, IMediator mediator, IMongoClient mongoClient)
    {
        this.mongoSettings = mongoSettings;
        this.mediator = mediator;
        this.mongoClient = mongoClient;

        this.insertManyOptions = new InsertManyOptions { IsOrdered = false };
    }

    protected async Task DispatchAndPersistDomainEventsAsync<T>(IClientSessionHandle session, IEntity<T> entity,
        CancellationToken cancellationToken = default) where T : IEntity<T>
    {
        foreach (var ev in entity.DomainEvents)
        {
            await this.mediator.Publish(ev, cancellationToken);
        }

        var eventDocuments = entity.DomainEvents
            .Select(domainEvent => domainEvent.ToMongoDocument())
            .ToList();

        var collection = this.mongoClient.GetDatabase(this.mongoSettings.Value.DatabaseName)
            .GetCollection<EventDocument>(Constants.EventsCollectionName);

        await collection.InsertManyAsync(session, eventDocuments, this.insertManyOptions, cancellationToken);
    }
}
