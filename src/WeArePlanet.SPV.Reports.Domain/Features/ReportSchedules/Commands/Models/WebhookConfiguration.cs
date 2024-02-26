using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class WebhookConfiguration : ValueObject<WebhookConfiguration>
{
    public WebhookConfiguration(Uri? onSuccess, Uri? onFailure)
    {
        this.OnSuccess = onSuccess;
        this.OnFailure = onFailure;
    }

    public Uri? OnSuccess { get; }
    public Uri? OnFailure { get; }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.OnFailure;
        yield return this.OnSuccess;
    }
}
