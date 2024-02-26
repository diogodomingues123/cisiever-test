using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Persistence.Common.Extensions;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Events;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules.Events;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.Common.Extensions;

public class DomainEventMapperExtensionsTests : PlanetTestClass
{
    public DomainEventMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_ValidReportCreationEvent_MapsSuccessfully()
    {
        // Arrange
        var ev = this.ObjectFactoryRegistry.Generate<ReportScheduleCreatedEvent>();
        var expected = new ReportScheduleCreatedEventDocument
        {
            Id = ev.Id.ToString(),
            ReportScheduleId = ev.Origin.Id!.ToString()!,
            TriggeredAt = StaticDateTimeProvider.CurrentDateTimeOffset.DateTime
        };

        // Act
        var document = ev.ToMongoDocument();

        // Assert
        document.Should().BeEquivalentTo(expected, options => options
            .Excluding(d => d.MongoId));
    }

    [Fact]
    public void ToMongoDocument_ValidReportUpdatedEvent_MapsSuccessfully()
    {
        // Arrange
        var ev = this.ObjectFactoryRegistry.Generate<ReportScheduleUpdatedEvent>();
        var expected = new ReportScheduleCreatedEventDocument
        {
            Id = ev.Id.ToString(),
            ReportScheduleId = ev.Origin.Id!.ToString()!,
            TriggeredAt = StaticDateTimeProvider.CurrentDateTimeOffset.DateTime
        };

        // Act
        var document = ev.ToMongoDocument();

        // Assert
        document.Should().BeEquivalentTo(expected, options => options
            .Excluding(d => d.MongoId));
    }

    [Fact]
    public void ToMongoDocument_ValidReportToggledEvent_MapsSuccessfully()
    {
        // Arrange
        var ev = this.ObjectFactoryRegistry.Generate<ReportScheduleToggledEvent>(ReportScheduleToggledEventFactory
            .ObjectNames.Activated);
        var expected = new ReportScheduleToggledEventDocument
        {
            Id = ev.Id.ToString(),
            ReportScheduleId = ev.Origin.Id!.ToString()!,
            TriggeredAt = StaticDateTimeProvider.CurrentDateTimeOffset.DateTime,
            Activated = ev.Origin.IsActive
        };

        // Act
        var document = ev.ToMongoDocument();

        // Assert
        document.Should().BeEquivalentTo(expected, options => options
            .Excluding(d => d.MongoId));
    }

    [Fact]
    public void ToMongoDocument_ValidReportArchivedEvent_MapsSuccessfully()
    {
        // Arrange
        var ev = this.ObjectFactoryRegistry.Generate<ReportScheduleArchivedEvent>();
        var expected = new ReportScheduleArchivedEventDocument
        {
            Id = ev.Id.ToString(),
            ReportScheduleId = ev.Origin.Id!.ToString()!,
            TriggeredAt = StaticDateTimeProvider.CurrentDateTimeOffset.DateTime,
            PreviousState = ev.PreviousState.Name
        };

        // Act
        var document = ev.ToMongoDocument();

        // Assert
        document.Should().BeEquivalentTo(expected, options => options
            .Excluding(d => d.MongoId));
    }

    [Fact]
    public void ToMongoDocument_NoTypeMapped_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = this.ObjectFactoryRegistry.Generate<IDomainEvent<DummyEntity>>();

        // Act
        var action = () => ev.ToMongoDocument();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}

public class DummyEntity : IEntity<DummyEntity>
{
    public object? Id => null!;
    public IReadOnlyCollection<IDomainEvent<DummyEntity>> DomainEvents => null!;
}
