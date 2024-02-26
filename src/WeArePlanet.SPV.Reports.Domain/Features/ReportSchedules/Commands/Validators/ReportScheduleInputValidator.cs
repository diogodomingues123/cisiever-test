using System.Collections.Immutable;

using FluentValidation;
using FluentValidation.Validators;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

public class ReportScheduleInputValidator : PropertyValidator<ReportSchedule, ReportScheduleInput?>
{
    public override string Name => nameof(ReportScheduleInputValidator);

    public override bool IsValid(ValidationContext<ReportSchedule> context, ReportScheduleInput? value)
    {
        return ValidateFields(context, value);
    }

    private static bool ValidateFields(ValidationContext<ReportSchedule> context,
        ReportScheduleInput? value)
    {
        if (value == null)
        {
            return true;
        }

        return context.InstanceToValidate.Template.InputFieldConfigurations
            .GroupJoin(
                value.Parameters,
                templateParameters => templateParameters.Key,
                receivedParameters => receivedParameters.Key,
                (templateParameter, receivedParameters) => (TemplateParameter: templateParameter,
                    ReceivedParameters: receivedParameters)
            )
            .SelectMany(
                templateAndReceivedParameterTuple =>
                    templateAndReceivedParameterTuple.ReceivedParameters.DefaultIfEmpty(),
                (templateParameter, receivedParameter) => (templateParameter.TemplateParameter,
                    ReceivedParameter: receivedParameter))
            .ToImmutableList()
            .ForAll(parameters => ValidateInputField(context, value, parameters.TemplateParameter.Value,
                parameters.ReceivedParameter.Value));
    }

    private static bool ValidateInputField(ValidationContext<ReportSchedule> context,
        ReportScheduleInput value, InputFieldConfiguration templateParameter, object? receivedParameter)
    {
        try
        {
            var result = templateParameter.FieldType.ExtractFrom(value, templateParameter.Name);

            if (result == null && templateParameter.IsRequired)
            {
                context.AddFailure(nameof(ReportSchedule.Input),
                    $"Required [{templateParameter.FieldType.Name}] field with name [{templateParameter.Name}] is either invalid or missing.");

                return false;
            }

            return true;
        }
        catch (InvalidOperationException)
        {
            context.AddFailure(nameof(ReportSchedule.Input),
                $"[{templateParameter.FieldType.Name}] field with name [{templateParameter.Name}] is invalid.");

            return false;
        }
    }
}
