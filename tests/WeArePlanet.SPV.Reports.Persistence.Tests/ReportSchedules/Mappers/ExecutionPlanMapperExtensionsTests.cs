using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.ReportSchedules.Mappers;

public class ExecutionPlanMapperExtensionsTests : PlanetTestClass
{
    public ExecutionPlanMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_OneOff_MapsSuccessfully()
    {
        // Arrange
        var plan = this.ObjectFactoryRegistry.Generate<OneOffReportExecutionPlan>();
        var expected = new ExecutionPlanDocument
        {
            Id = plan.Id!.Value!,
            Type = ScheduleDocumentType.OneOff,
            Date = plan.Date,
            TimeZoneId = TimeZoneId.Utc.Value
        };

        // Act
        var result = plan.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_OneOff_WithTimeZone_MapsSuccessfully()
    {
        // Arrange
        var plan = this.ObjectFactoryRegistry.Generate<OneOffReportExecutionPlan>(OneOffReportExecutionPlanFactory
            .ObjectNames.WithTimeZone);
        var expected = new ExecutionPlanDocument
        {
            Id = plan.Id!.Value!,
            Type = ScheduleDocumentType.OneOff,
            Date = plan.Date,
            TimeZoneId = plan.TimeZoneId.Value
        };

        // Act
        var result = plan.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_Recurring_MapsSuccessfully()
    {
        // Arrange
        var plan = this.ObjectFactoryRegistry.Generate<RecurringReportExecutionPlan>();
        var expected = new ExecutionPlanDocument
        {
            Id = plan.Id!.Value!,
            Type = ScheduleDocumentType.Recurring,
            Frequency = plan.Frequency,
            TimeZoneId = TimeZoneId.Utc.Value
        };

        // Act
        var result = plan.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_Recurring_WithTimezone_MapsSuccessfully()
    {
        // Arrange
        var plan = this.ObjectFactoryRegistry.Generate<RecurringReportExecutionPlan>(RecurringReportExecutionPlanFactory
            .ObjectNames.WithTimeZone);
        var expected = new ExecutionPlanDocument
        {
            Id = plan.Id!.Value!,
            Type = ScheduleDocumentType.Recurring,
            Frequency = plan.Frequency,
            TimeZoneId = plan.TimeZoneId.Value
        };

        // Act
        var result = plan.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_UnknownType_ThrowsInvalidOperationException()
    {
        // Arrange
        var plan = this.ObjectFactoryRegistry.Generate<IReportExecutionPlan>();

        // Act
        var result = () => plan.ToMongoDocument();

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ToMongoDocument_NullPlan_ThrowsArgumentNullException()
    {
        // Arrange
        IReportExecutionPlan? plan = null;

        // Act
        var result = () => plan!.ToMongoDocument();

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }
}
