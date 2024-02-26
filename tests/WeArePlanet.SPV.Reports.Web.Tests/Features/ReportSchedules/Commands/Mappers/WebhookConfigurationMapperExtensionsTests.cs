using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Commands.Mappers;

public class WebhookConfigurationMapperExtensionsTests : PlanetTestClass
{
    public WebhookConfigurationMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToDomainWebhookConfiguration_BothUris_ReturnsWebhookConfiguration()
    {
        // Arrange
        var onSuccess = new Uri("https://on-success");
        var onFailure = new Uri("https://on-failure");

        var webhookConfiguration = new WebhooksContract(onFailure, onSuccess);
        var expected = new WebhookConfiguration(onSuccess, onFailure);

        // Act
        var result = webhookConfiguration.ToDomainWebhookConfiguration();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainWebhookConfiguration_OnlySuccess_ReturnsWebhookConfiguration()
    {
        // Arrange
        var onSuccess = new Uri("https://on-success");

        var webhookConfiguration = new WebhooksContract(null, onSuccess);
        var expected = new WebhookConfiguration(onSuccess, null);

        // Act
        var result = webhookConfiguration.ToDomainWebhookConfiguration();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainWebhookConfiguration_OnlyFailure_ReturnsWebhookConfiguration()
    {
        // Arrange
        var onFailure = new Uri("https://on-failure");

        var webhookConfiguration = new WebhooksContract(onFailure, null);
        var expected = new WebhookConfiguration(null, onFailure);

        // Act
        var result = webhookConfiguration.ToDomainWebhookConfiguration();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainWebhookConfiguration_OnNull_ReturnsNull()
    {
        // Arrange
        WebhooksContract? webhookConfiguration = null;

        // Act
        var result = webhookConfiguration.ToDomainWebhookConfiguration();

        // Assert
        result.Should().BeNull();
    }
}
