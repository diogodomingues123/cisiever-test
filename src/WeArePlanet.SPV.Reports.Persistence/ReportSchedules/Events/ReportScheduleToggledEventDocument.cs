using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

using WeArePlanet.SPV.Reports.Persistence.Common;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;

[ExcludeFromCodeCoverage]
[BsonDiscriminator("ReportScheduleToggled")]
public class ReportScheduleToggledEventDocument : EventDocument
{
    [BsonElement("report_schedule_id")]
    public required string ReportScheduleId { get; set; }

    [BsonElement("activated")]
    public required bool Activated { get; set; }
}
