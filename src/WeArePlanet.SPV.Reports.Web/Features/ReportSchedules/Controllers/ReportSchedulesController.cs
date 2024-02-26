using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Extensions;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Queries.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Controllers;

[ApiController]
[Authorize]
public class ReportSchedulesController : Report_SchedulesControllerBase
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMediator mediator;

    public ReportSchedulesController(IMediator mediator, IDateTimeProvider dateTimeProvider)
    {
        this.mediator = mediator;
        this.dateTimeProvider = dateTimeProvider;
    }

    public override async Task<ActionResult<ReportSchedulePageContract>> GetReportSchedules(
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default)
    {
        var getReportSchedulesQuery = new GetReportSchedulesQuery(pageNumber, pageSize);
        var result = await this.mediator.Send(getReportSchedulesQuery, cancellationToken);

        return this.Ok(result.ToContractPage(this.dateTimeProvider));
    }

    public override async Task<ActionResult<ReportScheduleContract>> CreateReportSchedule(
        CreateReportScheduleRequestContract body,
        CancellationToken cancellationToken = default)
    {
        var result = await this.mediator.Send(body, cancellationToken);

        return result.Match<ObjectResult>(
            response => this.CreatedAtAction(
                nameof(this.GetReportSchedule),
                new { reportScheduleId = response.Id },
                response),
            validationResult => this.UnprocessableEntity(validationResult.ToProblemDetails()));
    }

    public override async Task<ActionResult<ReportScheduleContract>> GetReportSchedule(Guid reportScheduleId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetReportScheduleByIdQuery(new ReportScheduleId(reportScheduleId));
        var schedule = await this.mediator.Send(query, cancellationToken);

        return this.Ok(schedule.ToReportScheduleContract(this.dateTimeProvider));
    }

    public override async Task<ActionResult<ReportScheduleContract>> PatchReportSchedule(Guid reportScheduleId,
        PatchReportScheduleRequest body,
        CancellationToken cancellationToken = default)
    {
        body.Id = new ReportScheduleId(reportScheduleId);

        var result = await this.mediator.Send(body, cancellationToken);

        return result.Match<ObjectResult>(
            response => this.Ok(response),
            validationResult => this.UnprocessableEntity(validationResult.ToProblemDetails()));
    }

    public override async Task<IActionResult> ArchiveReportSchedule(Guid reportScheduleId,
        CancellationToken cancellationToken = default)
    {
        await this.mediator.Send(new ArchiveReportScheduleCommand(new ReportScheduleId(reportScheduleId)),
            cancellationToken);

        return this.NoContent();
    }
}
