using System.Diagnostics.CodeAnalysis;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Persistence.ReportTemplates;

[ExcludeFromCodeCoverage]
public class ReportTemplateConfigurationsRepository : IReportTemplateConfigurationsRepository
{
    private static readonly IReadOnlyDictionary<Guid, ReportTemplateConfiguration> TemplateConfigurations =
        FillTemplateConfigurations();

    public Task<Either<NotFoundException, ReportTemplateConfiguration>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        Either<NotFoundException, ReportTemplateConfiguration> result = TemplateConfigurations
            .TryGetValue(id, out var templateConfiguration)
            ? templateConfiguration
            : new NotFoundException($"Template [{id}] not found.");

        return Task.FromResult(result);
    }

    private static Dictionary<Guid, ReportTemplateConfiguration> FillTemplateConfigurations()
    {
        return new Dictionary<Guid, ReportTemplateConfiguration>
        {
            { Guid.Parse("1455563d-ae6b-4404-9dcf-004e1a855463"), CreateTemplateForOneOff() },
            { Guid.Parse("2455563d-ae6b-4404-9dcf-004e1a855463"), CreateTemplateForRecurring() }
        };
    }

    private static ReportTemplateConfiguration CreateTemplateForOneOff()
    {
        var inputConfigurations = new List<InputFieldConfiguration>
        {
            new("fromDate", true, new DateTimeFieldType()),
            new("toDate", true, new DateTimeFieldType()),
            new("timeZone", false, new StringFieldType())
        };

        var templateConfiguration = new ReportTemplateConfiguration
        (Guid.Parse("1455563d-ae6b-4404-9dcf-004e1a855463"),
            "CsvTemplate",
            "v1",
            ReportTemplateFormat.Csv,
            "{transactionId},{amount},{currency}"
        );

        foreach (var inputConfiguration in inputConfigurations)
        {
            templateConfiguration.AddInputFieldConfiguration(inputConfiguration);
        }

        return templateConfiguration;
    }

    private static ReportTemplateConfiguration CreateTemplateForRecurring()
    {
        var inputConfigurations = new List<InputFieldConfiguration>
        {
            new("fetchInterval", true, new EnumFieldType<FetchInterval>()),
            new("timeZone", false, new StringFieldType())
        };

        var templateConfiguration = new ReportTemplateConfiguration
        (Guid.Parse("2455563d-ae6b-4404-9dcf-004e1a855463"),
            "CsvTemplate",
            "v1",
            ReportTemplateFormat.Csv,
            "{transactionId},{amount},{currency}"
        );

        foreach (var inputConfiguration in inputConfigurations)
        {
            templateConfiguration.AddInputFieldConfiguration(inputConfiguration);
        }

        return templateConfiguration;
    }
}
