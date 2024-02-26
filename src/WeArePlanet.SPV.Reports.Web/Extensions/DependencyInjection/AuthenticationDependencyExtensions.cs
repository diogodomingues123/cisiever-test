using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Web.Auth;
using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class AuthenticationDependencyExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var auth0SettingsSection = configuration.GetRequiredSection(SettingsConstants.AuthSectionKey);
        var authSettings = serviceCollection.AddTypedOptionsAndGet<AuthSettings>(auth0SettingsSection).Value;
        var hangfireSettings = configuration.GetRequiredSection(SettingsConstants.HangfireSectionKey)
            .Get<HangfireSettings>();

        var authenticationBuilder = serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authSettings.Auth0.Domain.ToString();
                options.Audience = authSettings.Auth0.Audience;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
                options.Challenge = "Bearer";
            });

        if (hangfireSettings?.Dashboard.RequireAuthentication == true)
        {
            // If requires auth and okta is set, then configure OIDC and Cookies
            AddOktaAuthentication(authenticationBuilder, authSettings);
        }

        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(AuthConstants.HangfirePolicyName, policyOptions =>
            {
                // Properly set the requirements if hangfire dashboard auth is set.
                if (hangfireSettings?.Dashboard.RequireAuthentication ?? false)
                {
                    policyOptions.AddRequirements().RequireAuthenticatedUser();
                    policyOptions.AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme);
                }
                else
                {
                    policyOptions.AddRequirements(new AllowAnonymousRequirement());
                }
            });
        });

        serviceCollection.AddScoped<IAuthenticationPrincipal>(provider =>
            new AuthenticationPrincipal(provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User));

        serviceCollection.AddScoped<IPermissionService, MockPermissionService>();

        return serviceCollection;
    }

    private static void AddOktaAuthentication(AuthenticationBuilder authenticationBuilder,
        AuthSettings authSettings)
    {
        if (authSettings.Okta is null)
        {
            throw new InvalidOperationException(
                "Okta Settings must be configured when Hangfire.RequireAuthentication is true.");
        }

        authenticationBuilder
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // TODO: Check cookie settings. This was made to work, not to be actually right. Need to discuss this with someone who knows about cookie security.
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authSettings.Okta!.Domain.ToString();
                options.ClientId = authSettings.Okta.ClientId;
                options.ClientSecret = authSettings.Okta.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;

                options.SaveTokens = true;
                options.UsePkce = authSettings.Okta.UsePkce;
                options.UseTokenLifetime = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authSettings.Okta.Domain.ToString()
                };

                // Cookie Configuration for HTTP Deployments (only local when testing auth)
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;

                options.Scope.Clear();
                foreach (var scope in authSettings.Okta.Scopes)
                {
                    options.Scope.Add(scope);
                }
            });
    }
}
