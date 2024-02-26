using FluentValidation;

using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers.Validators;

public class PatchReportScheduleValidator : AbstractValidator<PatchReportScheduleRequest>
{
    public PatchReportScheduleValidator()
    {
        this
            .RuleFor(request => request.Id)
            .NotNull();
    }
}
