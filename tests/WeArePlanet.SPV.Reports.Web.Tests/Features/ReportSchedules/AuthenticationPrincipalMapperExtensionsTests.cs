using System.Security.Claims;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Common.Auth.Extensions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules;

public class AuthenticationPrincipalMapperExtensionsTests : PlanetTestClass
{
    public AuthenticationPrincipalMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToDomainOwner_MapsSuccessfully()
    {
        // Arrange
        var authPrincipal = new AuthenticationPrincipal(new ClaimsPrincipal(
            new[] { new ClaimsIdentity(new[] { new Claim("id", "abc"), new Claim("org_id", "ab2c") }) }
        ));

        var expected = new Owner("abc", "ab2c");

        // Act
        var result = authPrincipal.ToOwner();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
