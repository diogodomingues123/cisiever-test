using NSubstitute.ExceptionExtensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events.Handlers;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Events.Handlers;

public class ReportScheduleArchivedEventHandlerTests : PlanetTestClass
{
    private readonly IReportScheduler schedulerMock;
    private readonly ReportScheduleArchivedEventHandler sut;

    public ReportScheduleArchivedEventHandlerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.schedulerMock = Substitute.For<IReportScheduler>();
        this.sut = new ReportScheduleArchivedEventHandler(this.schedulerMock);
    }

    [Fact]
    public async Task Handle_ArchivedEvent_ActivePreviousState_DeschedulesReport()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var notification =
            this.ObjectFactoryRegistry.Generate<ReportScheduleArchivedEvent>(ReportScheduleArchivedEventFactory
                .ObjectNames.ActivatedPreviousState);

        // Act
        await this.sut.Handle(notification, cancellationToken);

        // Assert
        await this.schedulerMock.Received(1).DescheduleAsync(notification.Origin.ExecutionPlan.Id!, cancellationToken);
    }

    [Fact]
    public async Task Handle_ArchivedEvent_NotActiveReportSchedule_SkipsDeschedule()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var notification =
            this.ObjectFactoryRegistry.Generate<ReportScheduleArchivedEvent>();

        this.schedulerMock.DescheduleAsync(
                Arg.Any<ExecutionPlanId>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsyncForAnyArgs(new Exception("Should not try to deschedule the report."));

        // Act
        await this.sut.Handle(notification, cancellationToken);

        // Assert
        await this.schedulerMock.DidNotReceive()
            .DescheduleAsync(notification.Origin.ExecutionPlan.Id!, cancellationToken);
    }
}
