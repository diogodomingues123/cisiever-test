using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;

namespace WeArePlanet.SPV.Reports.Domain.Common.Auth;

public interface IPermissionService
{
    /// <summary>
    ///     Checks whether a <paramref name="principal" /> has access to the <paramref name="ownedResource" />.
    /// </summary>
    /// <param name="principal">The current request principal.</param>
    /// <param name="ownedResource">The resource owned by a principal.</param>
    /// <returns></returns>
    /// <exception cref="NoPermissionException">Thrown when the principal doesn't have access to the owned resource.</exception>
    Task CheckAccessAsync(IAuthenticationPrincipal principal, IOwned ownedResource);
}
