using System.Diagnostics.CodeAnalysis;

using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Exceptions;

[ExcludeFromCodeCoverage]
public class InputFieldConfigurationDuplicationException : Exception
{
    public InputFieldConfigurationDuplicationException(InputFieldConfiguration config)
        : base($"Cannot add duplicate input field {config.Name}.")
    {
    }
}
