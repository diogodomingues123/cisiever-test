using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

public class ReportScheduleToggledEventFactory : MultipleObjectFactory<ReportScheduleToggledEvent>
{
    public ReportScheduleToggledEventFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static IDictionary<string, Faker<ReportScheduleToggledEvent>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<ReportScheduleToggledEvent>>
        {
            {
                ObjectNames.Activated, new Faker<ReportScheduleToggledEvent>()
                    .CustomInstantiator(f => new ReportScheduleToggledEvent(
                        registry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                            .ReportScheduleWithOneOffExecutionPlan),
                        StaticDateTimeProvider.Instance))
            },
            {
                ObjectNames.Deactivated, new Faker<ReportScheduleToggledEvent>()
                    .CustomInstantiator(f => new ReportScheduleToggledEvent(
                        registry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames.OnInactiveState),
                        StaticDateTimeProvider.Instance))
            }
        };
    }

    public static class ObjectNames
    {
        public const string Activated = "Activated";
        public const string Deactivated = "Deactivated";
    }
}
