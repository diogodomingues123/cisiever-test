using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

[ExcludeFromCodeCoverage]
public class ExecutionPlanDocument
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.String)]
    public required string Id { get; set; }

    [BsonElement("type")]
    [BsonRepresentation(BsonType.String)]
    public ScheduleDocumentType Type { get; set; }

    [BsonElement("frequency")]
    public string? Frequency { get; set; }

    [BsonElement("date")]
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset? Date { get; set; }

    [BsonElement("time_zone")]
    public string? TimeZoneId { get; set; }
}

public enum ScheduleDocumentType
{
    OneOff = 1,
    Recurring = 2
}
