using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Documents;

public class ReportScheduleDocumentFactory : MultipleObjectFactory<ReportScheduleDocument>
{
    private readonly Faker<ReportScheduleDocument> faker;

    public ReportScheduleDocumentFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
        this.faker = new Faker<ReportScheduleDocument>()
            .CustomInstantiator(f => new ReportScheduleDocument
            {
                Id = f.Random.Guid().ToString(),
                Name = f.Name.JobArea(),
                Owner = registry.Generate<OwnerDocument>(),
                ExecutionPlan = registry.Generate<ExecutionPlanDocument>(),
                TemplateId = f.Random.Guid().ToString(),
                Input = new Dictionary<string, object> { { "abc", "cde" } },
                WebhookConfiguration = registry.Generate<WebhookConfigurationDocument>(),
                CreatedAt = f.Date.Recent(),
                UpdatedAt = f.Date.Recent(),
                State = ActiveReportScheduleState.Alias
            });
    }

    private static IDictionary<string, Faker<ReportScheduleDocument>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<ReportScheduleDocument>>
        {
            {
                string.Empty, new Faker<ReportScheduleDocument>()
                    .CustomInstantiator(f => new ReportScheduleDocument
                    {
                        Id = f.Random.Guid().ToString(),
                        Name = f.Name.JobArea(),
                        Owner = registry.Generate<OwnerDocument>(),
                        ExecutionPlan = registry.Generate<ExecutionPlanDocument>(),
                        TemplateId = f.Random.Guid().ToString(),
                        Input = new Dictionary<string, object> { { "abc", "cde" } },
                        WebhookConfiguration = registry.Generate<WebhookConfigurationDocument>(),
                        CreatedAt = f.Date.Recent(),
                        UpdatedAt = f.Date.Recent(),
                        State = ActiveReportScheduleState.Alias
                    })
            },
            {
                ObjectNames.WithTimeZone, new Faker<ReportScheduleDocument>()
                    .CustomInstantiator(f => new ReportScheduleDocument
                    {
                        Id = f.Random.Guid().ToString(),
                        Name = f.Name.JobArea(),
                        Owner = registry.Generate<OwnerDocument>(),
                        ExecutionPlan =
                            registry.Generate<ExecutionPlanDocument>(ExecutionPlanDocumentFactory.ObjectNames
                                .WithTimeZone),
                        TemplateId = f.Random.Guid().ToString(),
                        Input = new Dictionary<string, object> { { "abc", "cde" } },
                        WebhookConfiguration = registry.Generate<WebhookConfigurationDocument>(),
                        CreatedAt = f.Date.Recent(),
                        UpdatedAt = f.Date.Recent(),
                        State = ActiveReportScheduleState.Alias
                    })
            }
        };
    }

    public static class ObjectNames
    {
        public const string WithTimeZone = "WithTimeZone";
    }
}
