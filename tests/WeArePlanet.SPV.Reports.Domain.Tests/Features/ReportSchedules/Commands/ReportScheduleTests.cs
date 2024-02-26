using FluentValidation.Results;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State.Exceptions;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands;

public class ReportScheduleTests : PlanetTestClass
{
    public ReportScheduleTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task CreateAsync_WithValidBuilder_ReturnsReportConfiguration()
    {
        // Arrange
        var builder = Substitute.For<IReportScheduleBuilder>();
        var expected = this.ObjectFactoryRegistry.Generate<ReportSchedule>();

        builder.BuildAsync().Returns(expected);

        // Act
        var result = await ReportSchedule.CreateAsync(builder, StaticDateTimeProvider.Instance);

        // Assert
        result.IsRight.Should().BeTrue();
        result.IfRight(r =>
        {
            r.Should().BeEquivalentTo(expected);
            r.DomainEvents
                .Should().ContainSingle()
                .Which.Should().BeOfType<ReportScheduleCreatedEvent>()
                .Which.Origin.Should().Be(r);
        });
    }

    [Fact]
    public async Task CreateAsync_BuilderFails_ReturnsValidationResult()
    {
        // Arrange
        var builder = Substitute.For<IReportScheduleBuilder>();
        var expected = this.ObjectFactoryRegistry.Generate<ValidationResult>();

        builder.BuildAsync().Returns(expected);

        // Act
        var result = await ReportSchedule.CreateAsync(builder, StaticDateTimeProvider.Instance);

        // Assert
        result.IsLeft.Should().BeTrue();
        result.IfLeft(vr =>
        {
            vr.Should().BeEquivalentTo(expected);
        });
    }

    [Fact]
    public async Task UpdateAsync_WithValidBuilder_ReturnsReportConfiguration()
    {
        // Arrange
        var builder = Substitute.For<IReportScheduleBuilder>();
        var expected = this.ObjectFactoryRegistry.Generate<ReportSchedule>();

        builder.BuildAsync().Returns(expected);

        // Act
        var result = await ReportSchedule.UpdateAsync(builder, StaticDateTimeProvider.Instance);

        // Assert
        result.IsRight.Should().BeTrue();
        result.IfRight(r =>
        {
            r.Should().BeEquivalentTo(expected);
            r.DomainEvents
                .Should().ContainSingle()
                .Which.Should().BeOfType<ReportScheduleUpdatedEvent>()
                .Which.Origin.Should().Be(r);
        });
    }

    [Fact]
    public async Task UpdateAsync_BuilderFails_ReturnsValidationResult()
    {
        // Arrange
        var builder = Substitute.For<IReportScheduleBuilder>();
        var expected = this.ObjectFactoryRegistry.Generate<ValidationResult>();

        builder.BuildAsync().Returns(expected);

        // Act
        var result = await ReportSchedule.UpdateAsync(builder, StaticDateTimeProvider.Instance);

        // Assert
        result.IsLeft.Should().BeTrue();
        result.IfLeft(vr =>
        {
            vr.Should().BeEquivalentTo(expected);
        });
    }

    [Fact]
    public void ToggleActivation_ReportCanTransitionToState_NotActive_ChangesState()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithMockedState);

        instance.State.CanTransitionTo(Arg.Any<ActiveReportScheduleState>()).Returns(true);

        // Act
        instance.ToggleActivation(true, Substitute.For<IDateTimeProvider>());

        // Assert
        instance.State.Should().BeOfType<ActiveReportScheduleState>();
        instance.DomainEvents.Should().HaveCount(1);
        instance.DomainEvents.First().Should().BeOfType<ReportScheduleToggledEvent>()
            .Which.Origin.Should().Be(instance);
    }

    [Fact]
    public void ToggleActivation_ReportCannotTransitionToState_DoesNotChangeState()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithMockedState);

        instance.State.CanTransitionTo(Arg.Any<ActiveReportScheduleState>()).Returns(false);

        // Act
        var action = () => instance.ToggleActivation(true, Substitute.For<IDateTimeProvider>());

        // Assert
        action.Should().Throw<StateTransitionNotPossibleException>();
        instance.State.Should().NotBeOfType<ActiveReportScheduleState>();
        instance.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void ToggleActivation_ReportCanTransitionToState_ButIsAlreadyInDesiredState_DoesNotChangeState()
    {
        // Arrange
        var instance = this.ObjectFactoryRegistry.Generate<ReportSchedule>(); // Active schedule;

        // Act
        instance.ToggleActivation(true, Substitute.For<IDateTimeProvider>());

        // Assert
        instance.State.Should().BeOfType<ActiveReportScheduleState>();
        instance.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void ToggleActivation_Off_ReportCanTransitionToState_ButIsAlreadyInInactiveState_DoesNotChangeState()
    {
        // Arrange
        var instance = this.ObjectFactoryRegistry.Generate<ReportSchedule>(); // Active schedule;
        instance.ToggleActivation(false, Substitute.For<IDateTimeProvider>());

        // Act
        instance.ToggleActivation(false, Substitute.For<IDateTimeProvider>());

        // Assert
        instance.State.Should().BeOfType<InactiveReportScheduleState>();

        // Only one event should have been triggered
        instance.DomainEvents.Should().HaveCount(1);
        instance.DomainEvents.First().Should().BeOfType<ReportScheduleToggledEvent>()
            .Which.Origin.Should().Be(instance);
    }

    [Fact]
    public void Archive_ValidTransitionState_TransitionsSuccessfully()
    {
        // Arrange
        var instance = this.ObjectFactoryRegistry.Generate<ReportSchedule>(); // Active schedule;

        // Act
        instance.Archive(Substitute.For<IDateTimeProvider>());

        // Assert
        instance.State.Should().BeOfType<ArchivedReportScheduleState>();

        instance.DomainEvents.Should().HaveCount(1);
        instance.DomainEvents.First().Should().BeOfType<ReportScheduleArchivedEvent>()
            .Which.Origin.Should().Be(instance);
    }
}
