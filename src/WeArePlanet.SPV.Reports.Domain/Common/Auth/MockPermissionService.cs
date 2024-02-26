using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common.Auth;

[ExcludeFromCodeCoverage]
public class MockPermissionService : IPermissionService
{
    public Task CheckAccessAsync(IAuthenticationPrincipal principal,
        IOwned ownedResource)
    {
        // TODO: While we don't have a clear vision on how permissions will be asserted, let's ignore them for now
        return Task.CompletedTask;
    }
}
