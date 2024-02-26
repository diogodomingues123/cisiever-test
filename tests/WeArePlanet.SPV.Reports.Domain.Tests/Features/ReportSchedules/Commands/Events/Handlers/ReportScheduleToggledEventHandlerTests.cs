using NSubstitute.ExceptionExtensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleToggledEventHandlerTests : PlanetTestClass
{
    private readonly IReportScheduler schedulerMock;
    private readonly ReportScheduleToggledEventHandler sut;

    public ReportScheduleToggledEventHandlerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.schedulerMock = Substitute.For<IReportScheduler>();
        this.sut = new ReportScheduleToggledEventHandler(this.schedulerMock);
    }

    [Fact]
    public async Task Handle_DeactivatedEvent_DeschedulesReport()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var notification =
            this.ObjectFactoryRegistry.Generate<ReportScheduleToggledEvent>(ReportScheduleToggledEventFactory
                .ObjectNames.Deactivated);

        this.schedulerMock.ScheduleAsync(
                Arg.Any<ReportScheduleId>(),
                Arg.Any<DateTime>(),
                Arg.Any<TimeZoneId>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsyncForAnyArgs(new Exception("Should deschedule, not schedule the report."));

        // Act
        await this.sut.Handle(notification, cancellationToken);

        // Assert
        await this.schedulerMock.Received(1).DescheduleAsync(notification.Origin.ExecutionPlan.Id!, cancellationToken);
    }

    [Fact]
    public async Task Handle_ActivatedEvent_SchedulesReport()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var notification =
            this.ObjectFactoryRegistry.Generate<ReportScheduleToggledEvent>(ReportScheduleToggledEventFactory
                .ObjectNames.Activated);

        this.schedulerMock.DescheduleAsync(
                Arg.Any<ExecutionPlanId>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsyncForAnyArgs(new Exception("Should schedule, not deschedule the report."));

        var oneOffExecutionPlan = notification.Origin.ExecutionPlan as OneOffReportExecutionPlan;
        oneOffExecutionPlan.Should().NotBeNull();

        // Act
        await this.sut.Handle(notification, cancellationToken);

        // Assert
        await this.schedulerMock.Received(1)
            .ScheduleAsync(notification.Origin.Id!, oneOffExecutionPlan!.Date!.Value, oneOffExecutionPlan!.TimeZoneId,
                cancellationToken);
    }
}
