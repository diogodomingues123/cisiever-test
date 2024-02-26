using Bogus;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class OneOffReportExecutionPlanFactory : MultipleObjectFactory<OneOffReportExecutionPlan>
{
    public OneOffReportExecutionPlanFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static IDictionary<string, Faker<OneOffReportExecutionPlan>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<OneOffReportExecutionPlan>>
        {
            {
                string.Empty, new Faker<OneOffReportExecutionPlan>()
                    .CustomInstantiator(f =>
                        new OneOffReportExecutionPlan(new ExecutionPlanId(f.Random.Hash()),
                            f.Date.Soon().ToUniversalTime()))
            },
            {
                ObjectNames.WithTimeZone, new Faker<OneOffReportExecutionPlan>()
                    .CustomInstantiator(f =>
                        new OneOffReportExecutionPlan(new ExecutionPlanId(f.Random.Hash()),
                            f.Date.Soon().ToUniversalTime(),
                            new TimeZoneId("Europe/Lisbon")))
            }
        };
    }

    public static class ObjectNames
    {
        public const string WithTimeZone = "WithTimeZone";
    }
}
