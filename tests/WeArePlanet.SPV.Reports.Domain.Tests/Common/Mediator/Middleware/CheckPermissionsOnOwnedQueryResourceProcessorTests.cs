using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator.Middleware;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Common.Auth;

public class CheckPermissionsOnOwnedQueryResourceProcessorTests : PlanetTestClass
{
    private readonly IPermissionService permissionServiceMock;
    private readonly IAuthenticationPrincipal principalMock;

    private readonly CheckPermissionsOnOwnedQueryResourceProcessor<DummyOwnedRequest, DummyOwnedResponse> sutOwned;

    public CheckPermissionsOnOwnedQueryResourceProcessorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.principalMock = Substitute.For<IAuthenticationPrincipal>();
        this.permissionServiceMock = Substitute.For<IPermissionService>();

        this.sutOwned =
            new CheckPermissionsOnOwnedQueryResourceProcessor<DummyOwnedRequest, DummyOwnedResponse>(this.principalMock,
                this.permissionServiceMock);
    }

    [Fact]
    public async Task Process_OwnedResource_ChecksPermission()
    {
        // Arrange
        var request = new DummyOwnedRequest();
        var response = new DummyOwnedResponse();

        // Act
        await this.sutOwned.Process(request, response, CancellationToken.None);

        // Assert
        await this.permissionServiceMock
            .Received(1)
            .CheckAccessAsync(Arg.Is(this.principalMock), Arg.Is(response));
    }
}

internal class DummyNotOwnedRequest : IQuery<DummyNotOwnedResponse>
{
}

internal class DummyOwnedRequest : IQuery<DummyOwnedResponse>
{
}

internal class DummyNotOwnedResponse
{
}

internal class DummyOwnedResponse : IOwned
{
    public Owner Owner => new("user", "org");
}
