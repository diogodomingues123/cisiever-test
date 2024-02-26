using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class OwnerFactory : ObjectFactory<Owner>
{
    private readonly Faker<Owner> faker;

    public OwnerFactory(IObjectFactoryRegistry registry)
        : base(registry)
    {
        this.faker = new Faker<Owner>()
            .CustomInstantiator(f => new Owner(f.Random.Guid().ToString(), f.Random.Guid().ToString()));
    }

    public override Owner Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
