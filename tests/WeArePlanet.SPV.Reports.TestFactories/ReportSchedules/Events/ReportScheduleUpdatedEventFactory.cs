using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

public class ReportScheduleUpdatedEventFactory : ObjectFactory<ReportScheduleUpdatedEvent>
{
    private readonly Faker<ReportScheduleUpdatedEvent> faker;

    public ReportScheduleUpdatedEventFactory(IObjectFactoryRegistry registry) : base(registry)
    {
        this.faker = new Faker<ReportScheduleUpdatedEvent>()
            .CustomInstantiator(f => new ReportScheduleUpdatedEvent(
                this.Registry.Generate<ReportSchedule>(),
                StaticDateTimeProvider.Instance));
    }

    public override ReportScheduleUpdatedEvent Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
