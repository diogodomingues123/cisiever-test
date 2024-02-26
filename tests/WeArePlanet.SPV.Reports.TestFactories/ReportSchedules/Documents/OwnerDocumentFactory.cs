using Bogus;

using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Documents;

public class OwnerDocumentFactory : ObjectFactory<OwnerDocument>
{
    private readonly Faker<OwnerDocument> faker;

    public OwnerDocumentFactory(IObjectFactoryRegistry registry) : base(registry)
    {
        this.faker = new Faker<OwnerDocument>()
            .CustomInstantiator(f => new OwnerDocument
            {
                UserId = f.Random.Guid().ToString(), OrganizationId = f.Random.Guid().ToString()
            });
    }

    public override OwnerDocument Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
