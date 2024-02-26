using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Contracts;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

public static class WebhookConfigurationMapperExtensions
{
    public static WebhookConfiguration? ToDomainWebhookConfiguration(this WebhooksContract? webhooks)
    {
        return webhooks == null
            ? null
            : new WebhookConfiguration(webhooks.OnSuccess, webhooks.OnError);
    }

    public static WebhooksContract? ToContractWebhooks(this WebhookConfiguration? webhooks)
    {
        return webhooks == null
            ? null
            : new WebhooksContract(webhooks.OnFailure, webhooks.OnSuccess);
    }
}
