using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;
using WeArePlanet.SPV.Reports.Persistence.Common;
using WeArePlanet.SPV.Reports.Persistence.Common.Extensions;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;
using WeArePlanet.SPV.Reports.Persistence.Settings;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules;

[ExcludeFromCodeCoverage]
public class MongoReportScheduleRepository : MongoEntityRepository, IReportScheduleRepository
{
    private readonly ReplaceOptions addOrUpdateReplaceOptions;
    private readonly IOptions<PaginationSettings> paginationSettings;
    private readonly IReportTemplateConfigurationsRepository templateConfigurationsRepository;

    public MongoReportScheduleRepository(
        IMongoClient client,
        IOptions<MongoSettings> mongoOptions,
        IOptions<PaginationSettings> paginationSettings,
        IReportTemplateConfigurationsRepository templateConfigurationsRepository,
        IMediator mediator)
        : base(mongoOptions, mediator, client)
    {
        this.paginationSettings = paginationSettings;
        this.templateConfigurationsRepository = templateConfigurationsRepository;

        this.addOrUpdateReplaceOptions = new ReplaceOptions { IsUpsert = true };
    }

    public async Task AddOrUpdateAsync(ReportSchedule reportSchedule,
        CancellationToken cancellationToken = default)
    {
        using var session = await this.mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        var collection = this.GetReportScheduleCollection();

        try
        {
            session.StartTransaction();

            // Save the domain events
            await this.DispatchAndPersistDomainEventsAsync(session, reportSchedule, cancellationToken);

            var reportScheduleDocument = reportSchedule.ToMongoDocument();

            // Persist the report configuration
            await collection.ReplaceOneAsync(
                session,
                document => document.Id == reportScheduleDocument.Id,
                reportScheduleDocument,
                this.addOrUpdateReplaceOptions,
                cancellationToken);

            // If all goes well, then commit the transaction
            // TODO: If we can't commit the transaction, the report job will still be scheduled on hangfire since we don't share the transaction (Hangfire Mongo does not allow for transaction sharing). Maybe we can mitigate this by using the outbox pattern to write to hangfire out of process, but for simplicity let's use this simpler, less safe approach for now
            await session.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            if (session.IsInTransaction)
            {
                // Otherwise, abort the on-going transaction.
                // And since we don't want to cancel an abort if the user cancels the request, we don't use the received CancellationToken instance
                await session.AbortTransactionAsync(CancellationToken.None);
            }

            throw;
        }
    }

    public async Task<ReportScheduleProjection?> GetByIdAsync(ReportScheduleId requestId,
        CancellationToken cancellationToken = default)
    {
        var id = requestId.Value.ToString();

        var result = await this.GetReportScheduleCollection()
            .Find(document => document.Id == id && document.State != ArchivedReportScheduleState.Alias)
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return null;
        }

        var templateResult =
            await this.templateConfigurationsRepository.GetAsync(Guid.Parse(result.TemplateId), cancellationToken);

        return templateResult.Match(template => result.ToDomainProjection(template),
            notFound => result.ToDomainProjection());
    }

    public async Task<ReportScheduleProjection?> MarkAsExecuted(ReportScheduleId reportScheduleId,
        CancellationToken cancellationToken = default)
    {
        var id = reportScheduleId.Value.ToString();

        var result = await this.GetReportScheduleCollection().FindOneAndUpdateAsync(
            rs => rs.Id == id,
            Builders<ReportScheduleDocument>.Update.Set(rs => rs.State, ExecutedReportScheduleState.Alias),
            cancellationToken: cancellationToken);

        return result.ToDomainProjection();
    }

    public async Task<Page<ReportScheduleProjection>> GetAllAsync(GetReportSchedulesQuery query,
        IAuthenticationPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        // TODO: Handle authorization and fetching by owner
        var projectionPage = await this.GetReportScheduleCollection().AggregateByPage(
            Builders<ReportScheduleDocument>.Filter.Empty,
            Builders<ReportScheduleDocument>.Sort.Descending(doc => doc.UpdatedAt),
            query.PageNumber ?? 1,
            query.PageSize ?? this.paginationSettings.Value.DefaultPageSize);

        var mappedDomainList = new List<ReportScheduleProjection>(projectionPage.Entries.Count);
        foreach (var entry in projectionPage.Entries)
        {
            var entryTemplate =
                await this.templateConfigurationsRepository.GetAsync(Guid.Parse(entry.TemplateId), cancellationToken);

            // TODO: Handle situations where the template is not found (if that's possible, depending on the template management implementation)
            var projection = entryTemplate.Match(
                template => entry.ToDomainProjection(template),
                _ => entry.ToDomainProjection())!;

            mappedDomainList.Add(projection);
        }

        return new Page<ReportScheduleProjection>(mappedDomainList,
            projectionPage.PageNumber,
            projectionPage.PageSize,
            projectionPage.TotalItems);
    }

    private IMongoCollection<ReportScheduleDocument> GetReportScheduleCollection()
    {
        return this.mongoClient.GetDatabase(this.mongoSettings.Value.DatabaseName)
            .GetCollection<ReportScheduleDocument>(Constants.ReportSchedulesTableName);
    }
}
