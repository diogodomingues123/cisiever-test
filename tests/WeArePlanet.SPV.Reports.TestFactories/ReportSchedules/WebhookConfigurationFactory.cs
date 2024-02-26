using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class WebhookConfigurationFactory : ObjectFactory<WebhookConfiguration>
{
    private readonly Faker<WebhookConfiguration> faker;

    public WebhookConfigurationFactory(IObjectFactoryRegistry registry)
        : base(registry)
    {
        this.faker = new Faker<WebhookConfiguration>()
            .CustomInstantiator(f => new WebhookConfiguration(
                new Uri("api/v1/report-schedules", UriKind.Relative),
                new Uri("api/v2/report-schedules", UriKind.Relative)
            ));
    }

    public override WebhookConfiguration Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
