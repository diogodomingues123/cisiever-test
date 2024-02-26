namespace WeArePlanet.SPV.Reports.Domain.Common.Auth;

public interface IAuthenticationPrincipal
{
    string? UserId { get; }
    string? OrganizationId { get; }

    bool HasAccessTo(IOwned owned);
}
