using Bogus;

using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Documents;

public class ExecutionPlanDocumentFactory : MultipleObjectFactory<ExecutionPlanDocument>
{
    public ExecutionPlanDocumentFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static IDictionary<string, Faker<ExecutionPlanDocument>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<ExecutionPlanDocument>>
        {
            {
                string.Empty, new Faker<ExecutionPlanDocument>()
                    .CustomInstantiator(f => new ExecutionPlanDocument
                    {
                        Id = f.Random.Hash(), Date = f.Date.Soon(), Type = ScheduleDocumentType.OneOff
                    })
            },
            {
                ObjectNames.WithTimeZone, new Faker<ExecutionPlanDocument>()
                    .CustomInstantiator(f => new ExecutionPlanDocument
                    {
                        Id = f.Random.Hash(),
                        Date = f.Date.Soon(),
                        Type = ScheduleDocumentType.OneOff,
                        TimeZoneId = "America/New_York"
                    })
            }
        };
    }

    public static class ObjectNames
    {
        public const string WithTimeZone = "WithTimeZone";
    }
}
