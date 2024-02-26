using FluentValidation.Results;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common.Mediator;

// ReSharper disable once CheckNamespace Partial class to the generated code
namespace WeArePlanet.SPV.Reports.Web.Contracts;

public partial class CreateReportScheduleRequestContract : ICommand<Either<ValidationResult, ReportScheduleContract>>
{
}
