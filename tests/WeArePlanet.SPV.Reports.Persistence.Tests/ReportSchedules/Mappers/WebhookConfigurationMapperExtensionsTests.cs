using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.ReportSchedules.Mappers;

public class WebhookConfigurationMapperExtensionsTests : PlanetTestClass
{
    public WebhookConfigurationMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_ValidInstance_MapsSuccessfully()
    {
        // Arrange
        var webhooks = this.ObjectFactoryRegistry.Generate<WebhookConfiguration>();
        var expected =
            new WebhookConfigurationDocument { OnSuccess = webhooks.OnSuccess, OnFailure = webhooks.OnFailure };

        // Act
        var result = webhooks.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_NullProperties_MapsSuccessfully()
    {
        // Arrange
        var webhooks = new WebhookConfiguration(null, null);
        var expected = new WebhookConfigurationDocument();

        // Act
        var result = webhooks.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToMongoDocument_NullInstance_ReturnsNull()
    {
        // Arrange
        WebhookConfiguration? webhooks = null;

        // Act
        var result = webhooks.ToMongoDocument();

        // Assert
        result.Should().BeNull();
    }
}
