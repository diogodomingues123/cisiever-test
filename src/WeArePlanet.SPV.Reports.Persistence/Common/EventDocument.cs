using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;

namespace WeArePlanet.SPV.Reports.Persistence.Common;

[ExcludeFromCodeCoverage]
[BsonDiscriminator(RootClass = true, Required = true)]
[BsonKnownTypes(typeof(ReportScheduleCreatedEventDocument))]
[BsonKnownTypes(typeof(ReportScheduleUpdatedEventDocument))]
[BsonKnownTypes(typeof(ReportScheduleToggledEventDocument))]
public abstract class EventDocument
{
    [BsonId]
    public ObjectId? MongoId { get; set; }

    [BsonElement("event_id")]
    public required string Id { get; set; }

    [BsonElement("triggered_at")]
    public DateTime TriggeredAt { get; set; }
}
