using Bogus;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class RecurringReportExecutionPlanFactory : MultipleObjectFactory<RecurringReportExecutionPlan>
{
    public RecurringReportExecutionPlanFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static IDictionary<string, Faker<RecurringReportExecutionPlan>> CreateFakers(
        IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<RecurringReportExecutionPlan>>
        {
            {
                string.Empty, new Faker<RecurringReportExecutionPlan>()
                    .CustomInstantiator(f =>
                        new RecurringReportExecutionPlan(new ExecutionPlanId(f.Random.Hash()), "* * * * *", null))
            },
            {
                ObjectNames.WithTimeZone, new Faker<RecurringReportExecutionPlan>()
                    .CustomInstantiator(f =>
                        new RecurringReportExecutionPlan(new ExecutionPlanId(f.Random.Hash()), "* * * * *",
                            new TimeZoneId("Europe/Lisbon")))
            }
        };
    }

    public static class ObjectNames
    {
        public const string WithTimeZone = "WithTimeZone";
    }
}
