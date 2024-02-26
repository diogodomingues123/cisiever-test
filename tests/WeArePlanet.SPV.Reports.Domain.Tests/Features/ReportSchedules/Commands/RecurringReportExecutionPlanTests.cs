using FluentAssertions.Extensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands;

public class RecurringReportExecutionPlanTests : PlanetTestClass
{
    public RecurringReportExecutionPlanTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task ScheduleAsync_CallsRightScheduleMethod()
    {
        // Arrange
        var frequency = "* * * * *";
        var tz = new TimeZoneId("Europe/Moscow");
        var sut = new RecurringReportExecutionPlan(frequency, tz);

        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();
        var reportScheduler = Substitute.For<IReportScheduler>();
        var cancellationToken = new CancellationToken();
        var scheduleId = new ExecutionPlanId(Guid.NewGuid().ToString());

        reportScheduler.ScheduleRecurringAsync(
                Arg.Is(configuration.Id),
                Arg.Is(frequency),
                Arg.Is(tz),
                Arg.Is(cancellationToken))
            .Returns(Task.FromResult(scheduleId));

        // Act
        var executionPlanId = await sut.ScheduleAsync(configuration, reportScheduler, cancellationToken);

        // Assert
        executionPlanId.Should().BeEquivalentTo(scheduleId);
    }

    [Theory]
    [InlineData("* * * * *", "1994-07-22 11:32:00")]
    [InlineData("0 12 * * *", "1994-07-22 12:00:00")]
    [InlineData("0 0 30 * *", "1994-07-30 00:00:00")]
    [InlineData("30 15 30 * *", "1994-07-30 15:30:00")]
    public void GetNextExecutionDate_MultipleScenarios_AlwaysInUTC_ReturnsProperDate(string frequency,
        string expectedDateRaw)
    {
        // Arrange
        var dateTimeMock = Substitute.For<IDateTimeProvider>();
        var tz = new TimeZoneId("UTC");
        var currentDate = 22.July(1994).At(11.Hours().And(31.Minutes())).WithOffset(0.Hours());

        dateTimeMock.GetUtcNow().Returns(currentDate);

        var sut = new RecurringReportExecutionPlan(frequency, tz);
        var expectedDate = DateTime.Parse(expectedDateRaw);

        // Act
        var nextExecutionDate = sut.GetNextExecutionDateInUtc(dateTimeMock);

        // Assert
        nextExecutionDate.Should().Be(expectedDate);
    }

    [Theory]
    [InlineData("* * * * *", "2019-07-22 11:32:00", "Europe/Lisbon", 0)]
    [InlineData("0 12 * * *", "2019-07-23 11:00:00", "Europe/Lisbon", 0)]
    [InlineData("* * * * *", "2019-07-22 11:32:00", "Europe/Moscow", 0)]
    [InlineData("0 12 * * *", "2019-07-23 09:00:00", "Europe/Moscow", 0)]
    [InlineData("0 12 * * *", "2019-07-22 11:00:00", "Europe/Lisbon", 5)]
    [InlineData("0 12 * * *", "2019-07-22 09:00:00", "Europe/Moscow", 5)]
    public void GetNextExecutionDate_MultipleScenarios_InTimeZones_DaylightSavingsInPlace_ReturnsProperDate(
        string frequency,
        string expectedDateRaw, string timeZoneId, int localOffset)
    {
        // Arrange
        var dateTimeMock = Substitute.For<IDateTimeProvider>();
        var tz = new TimeZoneId(timeZoneId);
        var currentDate = 22.July(2019).At(11.Hours().And(31.Minutes())).WithOffset(localOffset.Hours());

        this.OutputHelper.WriteLine($"Current Local DateTime: {currentDate.DateTime}");
        this.OutputHelper.WriteLine($"Current UTC DateTime: {currentDate.UtcDateTime}");

        this.OutputHelper.WriteLine($"Desired Timezone: {timeZoneId}");
        this.OutputHelper.WriteLine($"Frequency: {frequency}");

        dateTimeMock.GetUtcNow().Returns(currentDate);

        var sut = new RecurringReportExecutionPlan(frequency, tz);
        var expectedDate = DateTime.Parse(expectedDateRaw);

        // Act
        var nextExecutionDate = sut.GetNextExecutionDateInUtc(dateTimeMock);

        // Assert
        this.OutputHelper.WriteLine($"Expected: {expectedDate}");
        this.OutputHelper.WriteLine($"Got: {nextExecutionDate}");

        nextExecutionDate.Should().Be(expectedDate);
    }

    [Theory]
    [InlineData("* * * * *", "2019-12-22 11:32:00", "Europe/Lisbon", 0)]
    [InlineData("0 12 * * *", "2019-12-22 12:00:00", "Europe/Lisbon", 0)]
    [InlineData("* * * * *", "2019-12-22 11:32:00", "Europe/Moscow", 0)]
    [InlineData("0 12 * * *", "2019-12-23 09:00:00", "Europe/Moscow", 0)]
    [InlineData("0 12 * * *", "2019-12-22 12:00:00", "Europe/Lisbon", 5)]
    [InlineData("0 12 * * *", "2019-12-22 09:00:00", "Europe/Moscow", 5)]
    public void GetNextExecutionDate_MultipleScenarios_InTimeZones_DaylightSavingsNotInPlace_ReturnsProperDate(
        string frequency,
        string expectedDateRaw, string timeZoneId, int localOffset)
    {
        // Arrange
        var dateTimeMock = Substitute.For<IDateTimeProvider>();
        var tz = new TimeZoneId(timeZoneId);
        var currentDate = 22.December(2019).At(11.Hours().And(31.Minutes())).WithOffset(localOffset.Hours());

        this.OutputHelper.WriteLine($"Current Local DateTime: {currentDate.DateTime}");
        this.OutputHelper.WriteLine($"Current UTC DateTime: {currentDate.UtcDateTime}");

        this.OutputHelper.WriteLine($"Desired Timezone: {timeZoneId}");
        this.OutputHelper.WriteLine($"Frequency: {frequency}");

        dateTimeMock.GetUtcNow().Returns(currentDate);

        var sut = new RecurringReportExecutionPlan(frequency, tz);
        var expectedDate = DateTime.Parse(expectedDateRaw);

        // Act
        var nextExecutionDate = sut.GetNextExecutionDateInUtc(dateTimeMock);

        // Assert
        this.OutputHelper.WriteLine($"Expected: {expectedDate}");
        this.OutputHelper.WriteLine($"Got: {nextExecutionDate}");

        nextExecutionDate.Should().Be(expectedDate);
    }
}
