using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.State;

public class InactiveReportScheduleStateTests : PlanetTestClass
{
    private readonly InactiveReportScheduleState sut;

    public InactiveReportScheduleStateTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new InactiveReportScheduleState();
    }

    [Fact]
    public void CanTransitionTo_ActiveState_ReturnsTrue()
    {
        // Arrange
        var desiredState = new ActiveReportScheduleState();

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
        var desiredState = new InactiveReportScheduleState();

        // Act
        var result = this.sut.CanTransitionTo(desiredState);

        // Assert
        result.Should().BeTrue();
    }
}
