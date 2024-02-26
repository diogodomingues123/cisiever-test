using Bogus;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class ReportScheduleProjectionFactory : ObjectFactory<ReportScheduleProjection>
{
    private readonly Faker<ReportScheduleProjection> faker;

    public ReportScheduleProjectionFactory(IObjectFactoryRegistry registry)
        : base(registry)
    {
        this.faker = new Faker<ReportScheduleProjection>()
            .CustomInstantiator(f => new ReportScheduleProjection
            {
                Id = new ReportScheduleId(f.Random.Guid()),
                Name = f.Name.JobType(),
                Owner = registry.Generate<Owner>(),
                Template = registry.Generate<ReportTemplateConfiguration>(),
                ExecutionPlan = registry.Generate<OneOffReportExecutionPlan>(),
                Input = registry.Generate<ReportScheduleInput>(),
                WebhookConfiguration = registry.Generate<WebhookConfiguration>(),
                CreatedAt = f.Date.Recent(2),
                UpdatedAt = f.Date.Recent(),
                State = new ActiveReportScheduleState()
            });
    }

    public override ReportScheduleProjection Generate(string? objectName = null)
    {
        return this.faker.Generate();
    }
}
