using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class ReportTemplateConfigurationFactory : ObjectFactory<ReportTemplateConfiguration>
{
    private readonly Faker<ReportTemplateConfiguration> faker;

    public ReportTemplateConfigurationFactory(IObjectFactoryRegistry registry)
        : base(registry)
    {
        this.faker = new Faker<ReportTemplateConfiguration>()
            .CustomInstantiator(f => new ReportTemplateConfiguration(
                f.Random.Guid(),
                f.Random.Hash(),
                f.Random.String(1),
                f.Random.Enum<ReportTemplateFormat>(),
                f.Random.Hash()
            ));
    }

    public override ReportTemplateConfiguration Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
