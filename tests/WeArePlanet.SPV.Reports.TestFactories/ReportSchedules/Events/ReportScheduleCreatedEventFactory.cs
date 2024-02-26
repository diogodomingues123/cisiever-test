using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

public class ReportScheduleCreatedEventFactory : ObjectFactory<ReportScheduleCreatedEvent>
{
    private readonly Faker<ReportScheduleCreatedEvent> faker;

    public ReportScheduleCreatedEventFactory(IObjectFactoryRegistry registry) : base(registry)
    {
        this.faker = new Faker<ReportScheduleCreatedEvent>()
            .CustomInstantiator(f => new ReportScheduleCreatedEvent(
                this.Registry.Generate<ReportSchedule>(),
                StaticDateTimeProvider.Instance));
    }

    public override ReportScheduleCreatedEvent Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
