using Bogus;

using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Documents;

public class WebhookConfigurationDocumentFactory : ObjectFactory<WebhookConfigurationDocument>
{
    private readonly Faker<WebhookConfigurationDocument> faker;

    public WebhookConfigurationDocumentFactory(IObjectFactoryRegistry registry) : base(registry)
    {
        this.faker = new Faker<WebhookConfigurationDocument>()
            .CustomInstantiator(f => new WebhookConfigurationDocument
            {
                OnSuccess = new Uri(f.Internet.Url()), OnFailure = new Uri(f.Internet.Url())
            });
    }

    public override WebhookConfigurationDocument Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
