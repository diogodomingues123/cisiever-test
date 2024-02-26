using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.State;

public class ExecutedReportScheduleStateTests : PlanetTestClass
{
    private readonly ExecutedReportScheduleState sut;

    public ExecutedReportScheduleStateTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new ExecutedReportScheduleState();
    }

    [Fact]
    public void CanTransitionTo_ActiveState_ReturnsFalse()
    {
        // Arrange
        var desiredState = new ActiveReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanTransitionTo_OwnState_ReturnsTrue()
    {
        // Arrange
        var desiredState = new ExecutedReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanTransitionTo_InactiveState_ReturnsFalse()
    {
        // Arrange
        var desiredState = new InactiveReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeFalse();
    }
}
