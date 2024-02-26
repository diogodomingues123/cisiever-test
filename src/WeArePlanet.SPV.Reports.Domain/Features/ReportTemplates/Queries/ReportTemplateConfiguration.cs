using System.Collections.Immutable;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Exceptions;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

public class ReportTemplateConfiguration
{
    public ReportTemplateConfiguration(Guid id, string name, string version, ReportTemplateFormat format,
        string template)
    {
        this.Id = id;
        this.Name = name;
        this.Version = version;
        this.Format = format;
        this.Template = template;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Version { get; }
    public ReportTemplateFormat Format { get; }
    public string Template { get; }

    public IDictionary<string, InputFieldConfiguration> InputFieldConfigurations { get; private set; } =
        ImmutableDictionary<string, InputFieldConfiguration>.Empty;

    public Either<InputFieldConfigurationDuplicationException, InputFieldConfiguration> AddInputFieldConfiguration(
        InputFieldConfiguration configuration)
    {
        // Not thread safe, but don't see the need
        if (ReferenceEquals(this.InputFieldConfigurations, ImmutableDictionary<string, InputFieldConfiguration>.Empty))
        {
            this.InputFieldConfigurations = new Dictionary<string, InputFieldConfiguration>();
        }

        if (this.InputFieldConfigurations.ContainsKey(configuration.Name))
        {
            return new InputFieldConfigurationDuplicationException(configuration);
        }

        this.InputFieldConfigurations.Add(configuration.Name, configuration);

        return configuration;
    }
}
