namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

public class InputFieldConfiguration
{
    public InputFieldConfiguration(string name, bool isRequired, IFieldType fieldType)
    {
        this.Name = name;
        this.IsRequired = isRequired;
        this.FieldType = fieldType;
    }

    public string Name { get; }
    public bool IsRequired { get; }
    public IFieldType FieldType { get; }
}
