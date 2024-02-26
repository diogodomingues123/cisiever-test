using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

[ExcludeFromCodeCoverage]
public class ReportScheduleDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? MongoId { get; init; }

    [BsonElement("schedule_id")]
    [BsonRequired]
    public required string Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("template_id")]
    public required string TemplateId { get; set; }

    [BsonElement("execution_plan")]
    public required ExecutionPlanDocument ExecutionPlan { get; set; }

    [BsonElement("owner")]
    public required OwnerDocument Owner { get; set; }

    [BsonElement("input")]
    [BsonIgnoreIfNull]
    public IDictionary<string, object>? Input { get; set; }

    [BsonElement("webhooks")]
    [BsonIgnoreIfNull]
    public WebhookConfigurationDocument? WebhookConfiguration { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("state")]
    public string State { get; set; }
}
