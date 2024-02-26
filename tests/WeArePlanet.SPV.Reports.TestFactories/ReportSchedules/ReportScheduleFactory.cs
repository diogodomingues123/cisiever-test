using System.Collections.Immutable;

using Bogus;

using FluentAssertions.Extensions;

using NSubstitute;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class ReportScheduleFactory : MultipleObjectFactory<ReportSchedule>
{
    public ReportScheduleFactory(IObjectFactoryRegistry registry)
        : base(registry, CreateFakers(registry))
    {
    }

    private static Dictionary<string, Faker<ReportSchedule>> CreateFakers(IObjectFactoryRegistry registry)
    {
        return new Dictionary<string, Faker<ReportSchedule>>
        {
            {
                string.Empty, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<IReportExecutionPlan>(),
                        new ActiveReportScheduleState()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>(),
                        Input = new ReportScheduleInput(ImmutableDictionary<string, object>.Empty)
                    })
            },
            {
                ObjectNames.ReportScheduleWithOneOffExecutionPlan, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<OneOffReportExecutionPlan>(),
                        new ActiveReportScheduleState()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>(),
                        Input = new ReportScheduleInput(ImmutableDictionary<string, object>.Empty)
                    })
            },
            {
                ObjectNames.ReportScheduleWithOneOffExecutionPlanAndNoInput, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<OneOffReportExecutionPlan>(),
                        new ActiveReportScheduleState()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>()
                    })
            },
            {
                ObjectNames.ReportScheduleWithMockedState, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<OneOffReportExecutionPlan>(),
                        Substitute.For<IReportScheduleState>()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>()
                    })
            },
            {
                ObjectNames.OnInactiveState, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<OneOffReportExecutionPlan>(),
                        new InactiveReportScheduleState()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>()
                    })
            },
            {
                ObjectNames.WithTimeZone, new Faker<ReportSchedule>()
                    .CustomInstantiator(f => new ReportSchedule(
                        new ReportScheduleId(f.Random.Guid()),
                        f.Name.JobType(),
                        registry.Generate<Owner>(),
                        registry.Generate<ReportTemplateConfiguration>(),
                        registry.Generate<OneOffReportExecutionPlan>(OneOffReportExecutionPlanFactory.ObjectNames
                            .WithTimeZone),
                        new ActiveReportScheduleState()
                    )
                    {
                        CreatedAt = 15.October(2023),
                        UpdatedAt = 16.October(2023),
                        WebhookConfiguration = registry.Generate<WebhookConfiguration>()
                    })
            }
        };
    }

    public static class ObjectNames
    {
        public const string ReportScheduleWithOneOffExecutionPlan = "ReportScheduleWithOneOffExecutionPlan";

        public const string ReportScheduleWithOneOffExecutionPlanAndNoInput =
            "ReportScheduleWithOneOffExecutionPlanAndNoInput";

        public const string ReportScheduleWithMockedState = "ReportScheduleWithMockedState";
        public const string OnInactiveState = "OnInactiveState";
        public const string WithTimeZone = "WithTimeZone";
    }
}
