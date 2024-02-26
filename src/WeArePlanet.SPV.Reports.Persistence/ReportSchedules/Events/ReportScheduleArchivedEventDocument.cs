using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

using WeArePlanet.SPV.Reports.Persistence.Common;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;

[ExcludeFromCodeCoverage]
[BsonDiscriminator("ReportScheduleArchived")]
public class ReportScheduleArchivedEventDocument : EventDocument
{
    [BsonElement("report_schedule_id")]
    public required string ReportScheduleId { get; set; }

    [BsonElement("previous_state")]
    public required string PreviousState { get; set; }
}
