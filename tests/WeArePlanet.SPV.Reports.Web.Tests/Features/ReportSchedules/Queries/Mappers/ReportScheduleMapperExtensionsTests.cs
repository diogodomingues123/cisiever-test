using FluentAssertions.Extensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Queries.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Queries.Mappers;

public class ReportScheduleMapperExtensionsTests : PlanetTestClass
{
    public ReportScheduleMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToReportScheduleContract_ReturnsProperInstance()
    {
        // Arrange
        var instance = new ReportScheduleProjection
        {
            Id = ReportScheduleId.Create(),
            Name = "abc",
            Owner = this.ObjectFactoryRegistry.Generate<Owner>(),
            Template = this.ObjectFactoryRegistry.Generate<ReportTemplateConfiguration>(),
            ExecutionPlan = this.ObjectFactoryRegistry.Generate<OneOffReportExecutionPlan>(),
            CreatedAt = 15.March(2023),
            UpdatedAt = 17.April(2023),
            Input = this.ObjectFactoryRegistry.Generate<ReportScheduleInput>(),
            WebhookConfiguration = this.ObjectFactoryRegistry.Generate<WebhookConfiguration>(),
            State = new ActiveReportScheduleState()
        };

        var expectedExecutionPlan = instance.ExecutionPlan as OneOffReportExecutionPlan;

        var expected = new ReportScheduleContract(
            instance.CreatedAt,
            new OneOffExecutionPlanContract(expectedExecutionPlan.Date,
                instance.ExecutionPlan.Id.Value,
                instance.ExecutionPlan.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance),
                expectedExecutionPlan.TimeZoneId.Value),
            instance.Id.Value,
            instance.Input.Parameters,
            instance.Name,
            ReportScheduleContractState.Active,
            instance.Template.Id,
            instance.UpdatedAt,
            new WebhooksContract(instance.WebhookConfiguration.OnFailure, instance.WebhookConfiguration.OnSuccess)
        );

        // Act
        var result = instance.ToReportScheduleContract(StaticDateTimeProvider.Instance);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToReportScheduleContract_NoInput_ReturnsProperInstance()
    {
        // Arrange
        var instance = new ReportScheduleProjection
        {
            Id = ReportScheduleId.Create(),
            Name = "abc",
            Owner = this.ObjectFactoryRegistry.Generate<Owner>(),
            Template = this.ObjectFactoryRegistry.Generate<ReportTemplateConfiguration>(),
            ExecutionPlan = this.ObjectFactoryRegistry.Generate<OneOffReportExecutionPlan>(),
            CreatedAt = 15.March(2023),
            UpdatedAt = 17.April(2023),
            WebhookConfiguration = this.ObjectFactoryRegistry.Generate<WebhookConfiguration>(),
            State = new ActiveReportScheduleState()
        };

        var expectedExecutionPlan = instance.ExecutionPlan as OneOffReportExecutionPlan;
        var expected = new ReportScheduleContract(
            instance.CreatedAt,
            new OneOffExecutionPlanContract(expectedExecutionPlan.Date,
                instance.ExecutionPlan.Id.Value,
                instance.ExecutionPlan.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance),
                expectedExecutionPlan.TimeZoneId.Value),
            instance.Id.Value,
            null,
            instance.Name,
            ReportScheduleContractState.Active,
            instance.Template.Id,
            instance.UpdatedAt,
            new WebhooksContract(instance.WebhookConfiguration.OnFailure, instance.WebhookConfiguration.OnSuccess)
        );

        // Act
        var result = instance.ToReportScheduleContract(StaticDateTimeProvider.Instance);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToReportScheduleContract_NoWebhooks_ReturnsProperInstance()
    {
        // Arrange
        var instance = new ReportScheduleProjection
        {
            Id = ReportScheduleId.Create(),
            Name = "abc",
            Owner = this.ObjectFactoryRegistry.Generate<Owner>(),
            Template = this.ObjectFactoryRegistry.Generate<ReportTemplateConfiguration>(),
            ExecutionPlan = this.ObjectFactoryRegistry.Generate<OneOffReportExecutionPlan>(),
            Input = this.ObjectFactoryRegistry.Generate<ReportScheduleInput>(),
            CreatedAt = 15.March(2023),
            UpdatedAt = 17.April(2023),
            State = new ExecutedReportScheduleState()
        };

        var expectedExecutionPlan = instance.ExecutionPlan as OneOffReportExecutionPlan;
        var expected = new ReportScheduleContract(
            instance.CreatedAt,
            new OneOffExecutionPlanContract(expectedExecutionPlan.Date,
                instance.ExecutionPlan.Id.Value,
                instance.ExecutionPlan.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance),
                expectedExecutionPlan.TimeZoneId.Value),
            instance.Id.Value,
            instance.Input.Parameters,
            instance.Name,
            ReportScheduleContractState.Executed,
            instance.Template.Id,
            instance.UpdatedAt,
            null
        );

        // Act
        var result = instance.ToReportScheduleContract(StaticDateTimeProvider.Instance);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
