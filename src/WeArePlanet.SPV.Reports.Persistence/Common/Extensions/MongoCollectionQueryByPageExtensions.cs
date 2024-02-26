using System.Diagnostics.CodeAnalysis;

using MongoDB.Driver;

using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Persistence.Common.Extensions;

// TODO: Remove when we have integration tests
[ExcludeFromCodeCoverage]
public static class MongoCollectionQueryByPageExtensions
{
    public static async Task<Page<TDocument>> AggregateByPage<TDocument>(
        this IMongoCollection<TDocument> collection,
        FilterDefinition<TDocument> filterDefinition,
        SortDefinition<TDocument> sortDefinition,
        int pageNumber,
        int pageSize)
    {
        var countFacet = AggregateFacet.Create("count",
            PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<TDocument>()
            }));

        var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<TDocument, TDocument>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(sortDefinition),
                PipelineStageDefinitionBuilder.Skip<TDocument>((pageNumber - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<TDocument>(pageSize)
            }));

        var aggregation = await collection.Aggregate()
            .Match(filterDefinition)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        var totalItems = aggregation.First()
            .Facets.First(result => result.Name == "count")
            .Output<AggregateCountResult>()?[0]
            ?.Count ?? 0;

        var entries = aggregation.First()
            .Facets.First(result => result.Name == "data")
            .Output<TDocument>();

        return new Page<TDocument>(entries, pageNumber, pageSize, totalItems);
    }
}
