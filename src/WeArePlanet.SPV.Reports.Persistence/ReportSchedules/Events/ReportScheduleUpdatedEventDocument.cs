using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

using WeArePlanet.SPV.Reports.Persistence.Common;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;

[ExcludeFromCodeCoverage]
[BsonDiscriminator("ReportScheduleUpdated")]
public class ReportScheduleUpdatedEventDocument : EventDocument
{
    [BsonElement("report_schedule_id")]
    public required string ReportScheduleId { get; set; }
}
