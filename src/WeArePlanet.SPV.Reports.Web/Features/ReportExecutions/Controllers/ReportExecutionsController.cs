using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportExecutions.Controllers;

[Authorize]
public class ReportExecutionsController : Report_ExecutionsControllerBase
{
    public override async Task<ActionResult<IEnumerable<ReportExecutionContract>>> GetReportExecutions(
        Guid reportScheduleId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<ActionResult<ReportExecutionContract>> GetReportExecution(Guid reportScheduleId,
        Guid reportExecutionId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<ActionResult<ArtifactContract>> GetReportExecutionArtifacts(Guid reportScheduleId,
        Guid reportExecutionId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
