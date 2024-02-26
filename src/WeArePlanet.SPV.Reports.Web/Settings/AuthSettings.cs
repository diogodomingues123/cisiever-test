using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Web.Settings;

[ExcludeFromCodeCoverage]
public class AuthSettings
{
    public required IdentityProviderSettings? Okta { get; init; }

    [Required]
    public required IdentityProviderSettings Auth0 { get; init; }
}

[ExcludeFromCodeCoverage]
public class IdentityProviderSettings
{
    [Required]
    public required Uri Domain { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string ClientId { get; init; }

    [Required]
    public required string ClientSecret { get; init; }

    [Required]
    public required string[] Scopes { get; init; } = { "openid", "profile", "email", "groups" };

    [Required]
    public required bool UsePkce { get; init; } = true;
}
