using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

[ExcludeFromCodeCoverage]
public class WebhookConfigurationDocument
{
    [BsonElement("on_success")]
    public Uri? OnSuccess { get; init; }

    [BsonElement("on_failure")]
    public Uri? OnFailure { get; init; }
}
