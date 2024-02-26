using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.ReportSchedules.Mappers;

public class OwnerMapperExtensionsTests : PlanetTestClass
{
    public OwnerMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_ValidInstance_MapsSuccessfully()
    {
        // Arrange
        var owner = this.ObjectFactoryRegistry.Generate<Owner>();
        var expected = new OwnerDocument { UserId = owner.UserId, OrganizationId = owner.OrganizationId };

        // Act
        var result = owner.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_NullProperties_MapsSuccessfully()
    {
        // Arrange
        var owner = new Owner(null, null);
        var expected = new OwnerDocument();

        // Act
        var result = owner.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_NullInstance_ThrowsArgumentNullException()
    {
        // Arrange
        Owner? owner = null;

        // Act
        var result = () => owner.ToMongoDocument();

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }
}
