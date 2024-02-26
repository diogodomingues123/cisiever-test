using FluentValidation.Results;

using LanguageExt;

using Newtonsoft.Json;

using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

// ReSharper disable once CheckNamespace Partial class to the generated code
namespace WeArePlanet.SPV.Reports.Web.Contracts;

public partial class PatchReportScheduleRequest : ICommand<Either<ValidationResult, ReportScheduleContract>>
{
    [JsonIgnore]
    public ReportScheduleId? Id { get; set; }

    public bool IsEmpty()
    {
        return this.Active == null;
    }
}
