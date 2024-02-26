using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class ReportScheduleId : Id<ReportSchedule, Guid>
{
    public ReportScheduleId(Guid value) : base(value)
    {
    }

    public static ReportScheduleId Create()
    {
        return new ReportScheduleId(Guid.NewGuid());
    }
}
