using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class WebhookConfigurationMapperExtensions
{
    public static WebhookConfigurationDocument? ToMongoDocument(this WebhookConfiguration? webhookConfiguration)
    {
        if (webhookConfiguration == null)
        {
            return null;
        }

        return new WebhookConfigurationDocument
        {
            OnSuccess = webhookConfiguration.OnSuccess, OnFailure = webhookConfiguration.OnFailure
        };
    }

    public static WebhookConfiguration? ToDomainWebhookConfiguration(
        this WebhookConfigurationDocument? webhookConfiguration)
    {
        return webhookConfiguration == null
            ? null
            : new WebhookConfiguration(webhookConfiguration.OnSuccess, webhookConfiguration.OnFailure);
    }
}
