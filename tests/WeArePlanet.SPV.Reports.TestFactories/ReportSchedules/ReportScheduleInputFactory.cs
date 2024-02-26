using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class ReportScheduleInputFactory : ObjectFactory<ReportScheduleInput>
{
    private readonly Faker<ReportScheduleInput> faker;

    public ReportScheduleInputFactory(IObjectFactoryRegistry registry)
        : base(registry)
    {
        this.faker = new Faker<ReportScheduleInput>()
            .CustomInstantiator(f => new ReportScheduleInput(new Dictionary<string, object>
            {
                { "field1", 1 }, { "field2", "abc" }
            }));
    }

    public override ReportScheduleInput Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
