using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

public class ReportScheduleArchivedEventFactory : MultipleObjectFactory<ReportScheduleArchivedEvent>
{
    public ReportScheduleArchivedEventFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static IDictionary<string, Faker<ReportScheduleArchivedEvent>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<ReportScheduleArchivedEvent>>
        {
            {
                string.Empty, new Faker<ReportScheduleArchivedEvent>()
                    .CustomInstantiator(f => new ReportScheduleArchivedEvent(
                        registry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                            .ReportScheduleWithOneOffExecutionPlan),
                        new ArchivedReportScheduleState(),
                        StaticDateTimeProvider.Instance))
            },
            {
                ObjectNames.ActivatedPreviousState, new Faker<ReportScheduleArchivedEvent>()
                    .CustomInstantiator(f => new ReportScheduleArchivedEvent(
                        registry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                            .ReportScheduleWithOneOffExecutionPlan),
                        new ActiveReportScheduleState(),
                        StaticDateTimeProvider.Instance))
            }
        };
    }

    public static class ObjectNames
    {
        public const string ActivatedPreviousState = "ActivatedPreviousState";
    }
}
