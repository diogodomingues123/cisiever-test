using FluentAssertions.Extensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands;

public class OneOffReportExecutionPlanTests : PlanetTestClass
{
    public OneOffReportExecutionPlanTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task ScheduleAsync_SchedulesReport()
    {
        // Arrange
        var date = 1.October(2023).At(15.Hours());
        var tz = new TimeZoneId("Europe/Lisbon");
        var oneOffSchedule = new OneOffReportExecutionPlan(date, tz);

        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();
        var reportScheduler = Substitute.For<IReportScheduler>();
        var cancellationToken = new CancellationToken();
        var executionPlanId = new ExecutionPlanId(Guid.NewGuid().ToString());

        reportScheduler.ScheduleAsync(
                Arg.Is(configuration.Id),
                Arg.Is(date),
                Arg.Is(tz),
                Arg.Is(cancellationToken))
            .Returns(Task.FromResult(executionPlanId));

        // Act
        var scheduleIdResult = await oneOffSchedule.ScheduleAsync(configuration, reportScheduler, cancellationToken);

        // Assert
        scheduleIdResult.Should().BeEquivalentTo(executionPlanId);
    }

    [Fact]
    public async Task ScheduleAsync_NoDate_TriggersReport()
    {
        // Arrange
        var oneOffSchedule = new OneOffReportExecutionPlan();

        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();
        var reportScheduler = Substitute.For<IReportScheduler>();
        var cancellationToken = new CancellationToken();
        var executionPlanId = new ExecutionPlanId(Guid.NewGuid().ToString());

        reportScheduler.TriggerAsync(
                Arg.Is(configuration.Id),
                Arg.Is(cancellationToken))
            .Returns(Task.FromResult(executionPlanId));

        // Act
        var scheduleIdResult = await oneOffSchedule.ScheduleAsync(configuration, reportScheduler, cancellationToken);

        // Assert
        scheduleIdResult.Should().BeEquivalentTo(executionPlanId);
    }

    [Fact]
    public void GetNextExecutionDate_NoDate_ReturnsNow()
    {
        // Arrange
        var oneOffSchedule = new OneOffReportExecutionPlan();

        // Act
        var nextExecutionDate = oneOffSchedule.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance);

        // Assert
        nextExecutionDate.Should().Be(StaticDateTimeProvider.CurrentDateTimeOffset.UtcDateTime);
    }

    [Fact]
    public void GetNextExecutionDate_WithDate_NoTimeZone_DefaultsToUtc_ReturnsDate()
    {
        // Arrange
        var oneOffSchedule =
            new OneOffReportExecutionPlan(15.April(2023).At(23.Hours().And(15.Minutes())).ToUniversalTime(), null);

        // Act
        var nextExecutionDate = oneOffSchedule.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance);

        // Assert
        nextExecutionDate.Should().Be(oneOffSchedule.Date);
    }

    [Fact]
    public void GetNextExecutionDate_WithDate_WithTimeZone_DefaultsToUtc_ReturnsDate()
    {
        // Arrange
        var tz = new TimeZoneId("Europe/Moscow");
        var oneOffSchedule =
            new OneOffReportExecutionPlan(15.April(2023).At(23.Hours().And(15.Minutes())), tz);

        // Act
        var expectedExecutionDate = TimeZoneInfo.ConvertTimeToUtc(oneOffSchedule.Date!.Value, tz);
        var nextExecutionDate = oneOffSchedule.GetNextExecutionDateInUtc(StaticDateTimeProvider.Instance);

        // Assert
        nextExecutionDate.Should().Be(expectedExecutionDate);
    }
}
