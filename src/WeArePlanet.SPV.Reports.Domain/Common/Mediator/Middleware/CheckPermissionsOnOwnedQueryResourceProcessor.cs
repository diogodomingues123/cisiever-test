using MediatR.Pipeline;

using WeArePlanet.SPV.Reports.Domain.Common.Auth;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator.Middleware;

public class
    CheckPermissionsOnOwnedQueryResourceProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
    where TResponse : IOwned
{
    private readonly IPermissionService permissionService;
    private readonly IAuthenticationPrincipal principal;

    public CheckPermissionsOnOwnedQueryResourceProcessor(IAuthenticationPrincipal principal,
        IPermissionService permissionService)
    {
        this.principal = principal;
        this.permissionService = permissionService;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        await this.permissionService.CheckAccessAsync(this.principal, response);
    }
}
