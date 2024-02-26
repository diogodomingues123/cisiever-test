using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.ReportSchedules.Mappers;

public class ReportScheduleProjectionMapperExtensionsTests : PlanetTestClass
{
    public ReportScheduleProjectionMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_ValidInstance_MapsSuccessfully()
    {
        // Arrange
        var instance = this.ObjectFactoryRegistry.Generate<ReportScheduleDocument>();
        var expected = new ReportScheduleProjection
        {
            Id = new ReportScheduleId(Guid.Parse(instance.Id)),
            Name = instance.Name,
            CreatedAt = instance.CreatedAt,
            UpdatedAt = instance.UpdatedAt,
            Owner = new Owner(instance.Owner.UserId, instance.Owner.OrganizationId),
            ExecutionPlan =
                new OneOffReportExecutionPlan(new ExecutionPlanId(instance.ExecutionPlan.Id),
                    instance.ExecutionPlan.Date?.Date,
                    TimeZoneId.Utc),
            Input = new ReportScheduleInput(instance.Input),
            WebhookConfiguration = new WebhookConfiguration(instance.WebhookConfiguration!.OnSuccess,
                instance.WebhookConfiguration.OnFailure),
            Template = null!, // Don't assert the template
            State = new ActiveReportScheduleState()
        };

        // Act
        var result = instance.ToDomainProjection();

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(s => s.Template));
    }

    [Fact]
    public void ToMongoDocument_ValidInstance_WithTimeZone_MapsSuccessfully()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportScheduleDocument>(ReportScheduleFactory.ObjectNames.WithTimeZone);
        var expected = new ReportScheduleProjection
        {
            Id = new ReportScheduleId(Guid.Parse(instance.Id)),
            Name = instance.Name,
            CreatedAt = instance.CreatedAt,
            UpdatedAt = instance.UpdatedAt,
            Owner = new Owner(instance.Owner.UserId, instance.Owner.OrganizationId),
            ExecutionPlan =
                new OneOffReportExecutionPlan(new ExecutionPlanId(instance.ExecutionPlan.Id),
                    instance.ExecutionPlan.Date?.Date,
                    new TimeZoneId(instance.ExecutionPlan.TimeZoneId!)),
            Input = new ReportScheduleInput(instance.Input),
            WebhookConfiguration = new WebhookConfiguration(instance.WebhookConfiguration!.OnSuccess,
                instance.WebhookConfiguration.OnFailure),
            Template = null!, // Don't assert the template
            State = new ActiveReportScheduleState()
        };

        // Act
        var result = instance.ToDomainProjection();

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(s => s.Template));
    }

    [Fact]
    public void ToMongoDocument_NoInput_MapsSuccessfully()
    {
        // Arrange
        var instance = this.ObjectFactoryRegistry.Generate<ReportScheduleDocument>();

        instance.Input = null;

        var expected = new ReportScheduleProjection
        {
            Id = new ReportScheduleId(Guid.Parse(instance.Id)),
            Name = instance.Name,
            CreatedAt = instance.CreatedAt,
            UpdatedAt = instance.UpdatedAt,
            Owner = new Owner(instance.Owner.UserId, instance.Owner.OrganizationId),
            ExecutionPlan =
                new OneOffReportExecutionPlan(new ExecutionPlanId(instance.ExecutionPlan.Id),
                    instance.ExecutionPlan.Date!.Value.Date),
            Input = null,
            WebhookConfiguration = new WebhookConfiguration(instance.WebhookConfiguration!.OnSuccess,
                instance.WebhookConfiguration.OnFailure),
            Template = null!,
            State = new ActiveReportScheduleState()
        };

        // Act
        var result = instance.ToDomainProjection();

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Including(s => s.Input));
    }
}
