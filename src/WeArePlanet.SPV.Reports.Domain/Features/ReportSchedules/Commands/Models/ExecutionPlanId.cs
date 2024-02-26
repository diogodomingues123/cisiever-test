using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class ExecutionPlanId : Id<IReportExecutionPlan, string>
{
    public ExecutionPlanId(string value) : base(value)
    {
    }
}
