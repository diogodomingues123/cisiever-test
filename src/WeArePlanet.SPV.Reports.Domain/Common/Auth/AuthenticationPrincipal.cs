using System.Security.Claims;

using WeArePlanet.SPV.Reports.Domain.Common.Auth.Extensions;

namespace WeArePlanet.SPV.Reports.Domain.Common.Auth;

public class AuthenticationPrincipal : IAuthenticationPrincipal
{
    private readonly ClaimsPrincipal? principal;

    public AuthenticationPrincipal(ClaimsPrincipal? principal)
    {
        this.principal = principal;
    }

    public string? UserId => this.principal?.FindFirstValue("id");
    public string? OrganizationId => this.principal?.FindFirstValue("org_id");

    public bool HasAccessTo(IOwned owned)
    {
        return this.ToOwner().Equals(owned.Owner);
    }
}
