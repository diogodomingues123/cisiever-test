using Microsoft.AspNetCore.Authorization;

namespace WeArePlanet.SPV.Reports.Web.Auth;

public class AllowAnonymousRequirement : AuthorizationHandler<AllowAnonymousRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AllowAnonymousRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
