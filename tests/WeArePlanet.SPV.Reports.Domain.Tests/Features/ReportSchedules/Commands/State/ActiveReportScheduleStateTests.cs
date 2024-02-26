using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.State;

public class ActiveReportScheduleStateTests : PlanetTestClass
{
    private readonly ActiveReportScheduleState sut;

    public ActiveReportScheduleStateTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new ActiveReportScheduleState();
    }

    [Fact]
    public void CanTransitionTo_InactiveState_ReturnsTrue()
    {
        // Arrange
        var desiredState = new InactiveReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanTransitionTo_ExecutedState_ReturnsTrue()
    {
        // Arrange
        var desiredState = new ExecutedReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanTransitionTo_OwnState_ReturnsTrue()
    {
        // Arrange
        var desiredState = new ActiveReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeTrue();
    }
}
