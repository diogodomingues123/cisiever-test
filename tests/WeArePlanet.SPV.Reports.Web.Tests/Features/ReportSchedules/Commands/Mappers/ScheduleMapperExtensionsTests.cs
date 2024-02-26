using FluentAssertions.Extensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Commands.Mappers;

public class ScheduleMapperExtensionsTests : PlanetTestClass
{
    public ScheduleMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToDomainReportSchedule_OneOffSchedule_MapsCorrectly()
    {
        // Arrange
        var date = 10.April(2023).At(15.Hours().And(30.Minutes()));
        var nextExecutionDate = 11.April(2023).At(15.Hours().And(30.Minutes()));
        var schedule = new OneOffExecutionPlanContract(date, string.Empty, nextExecutionDate, null);
        var expected = new OneOffReportExecutionPlan(date, null);

        // Act
        var result = schedule.ToDomainReportExecutionPlan();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainReportSchedule_RecurringSchedule_MapsCorrectly()
    {
        // Arrange
        var cron = "* * * * *";
        var nextExecutionDate = 11.April(2023).At(15.Hours().And(30.Minutes()));
        var schedule = new RecurringExecutionPlanContract(cron, string.Empty, nextExecutionDate, null);
        var expected = new RecurringReportExecutionPlan(cron, null);

        // Act
        var result = schedule.ToDomainReportExecutionPlan();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainReportSchedule_RecurringSchedule_WithTimeZone_MapsCorrectly()
    {
        // Arrange
        var cron = "* * * * *";
        var nextExecutionDate = 11.April(2023).At(15.Hours().And(30.Minutes()));
        var schedule = new RecurringExecutionPlanContract(cron, string.Empty, nextExecutionDate, "Europe/Lisbon");
        var expected = new RecurringReportExecutionPlan(cron, new TimeZoneId("Europe/Lisbon"));

        // Act
        var result = schedule.ToDomainReportExecutionPlan();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainReportSchedule_OneOffSchedule_WithTimeZone_MapsCorrectly()
    {
        // Arrange
        var date = 10.April(2023).At(15.Hours().And(30.Minutes()));
        var nextExecutionDate = 11.April(2023).At(15.Hours().And(30.Minutes()));
        var schedule = new OneOffExecutionPlanContract(date, string.Empty, nextExecutionDate, "Europe/Lisbon");
        var expected = new OneOffReportExecutionPlan(date, new TimeZoneId("Europe/Lisbon"));

        // Act
        var result = schedule.ToDomainReportExecutionPlan();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainReportSchedule_InvalidSchedule_ReturnsNull()
    {
        // Arrange
        var schedule = new InvalidExecutionPlan();

        // Act
        var result = schedule.ToDomainReportExecutionPlan();

        // Assert
        result.Should().BeNull();
    }

    private class InvalidExecutionPlan : ExecutionPlanContract
    {
        public InvalidExecutionPlan() : base(string.Empty, DateTime.UtcNow)
        {
        }
    }
}
