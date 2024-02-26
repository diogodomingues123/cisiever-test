using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

[ExcludeFromCodeCoverage]
public class OwnerDocument
{
    [BsonElement("user_id")]
    public string? UserId { get; set; }

    [BsonElement("org_id")]
    public string? OrganizationId { get; set; }
}
