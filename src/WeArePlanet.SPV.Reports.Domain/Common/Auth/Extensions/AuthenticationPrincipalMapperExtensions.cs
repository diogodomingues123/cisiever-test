using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Common.Auth.Extensions;

public static class AuthenticationPrincipalMapperExtensions
{
    public static Owner ToOwner(this IAuthenticationPrincipal principal)
    {
        return new Owner(principal.UserId!, principal.OrganizationId!);
    }
}
