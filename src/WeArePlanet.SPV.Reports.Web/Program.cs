using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hangfire;

using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator.Middleware;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Handlers;
using WeArePlanet.SPV.Reports.Persistence.Settings;
using WeArePlanet.SPV.Reports.Web.Auth;
using WeArePlanet.SPV.Reports.Web.Extensions;
using WeArePlanet.SPV.Reports.Web.Extensions.DependencyInjection;
using WeArePlanet.SPV.Reports.Web.Features.ReportExecutions.DependencyInjection;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers.Validators;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.DependencyInjection;
using WeArePlanet.SPV.Reports.Web.Features.ReportTemplates.DependencyInjection;
using WeArePlanet.SPV.Reports.Web.HealthChecks;
using WeArePlanet.SPV.Reports.Web.Settings;

namespace WeArePlanet.SPV.Reports.Web;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        Configure(app);

        await app.RunAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddEnvironmentVariables(options => options.Prefix = "REPORTSAPI_");

        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ProblemDetailsConverter());
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                options.SerializerSettings.MaxDepth = 24;
            })
            .AddProblemDetailsConventions();

        builder.Services.AddTypedOptions<PaginationSettings>(builder.Configuration,
            SettingsConstants.PaginationSectionKey);

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;

            options.KnownProxies.Clear();
            options.KnownNetworks.Clear();
        });

        var healthCheckBuilder = builder.Services.AddHealthChecks();

        builder.Services
            .AddHttpContextAccessor()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddOpenApiServices(builder.Configuration)
            .AddProblemDetails(options =>
            {
                options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
                options.MapToStatusCode<NoPermissionException>(StatusCodes.Status403Forbidden);
                options.MapToStatusCode<StateTransitionNotPossibleException>(StatusCodes.Status400BadRequest);
                options.MapToStatusCode<ValidationException>(StatusCodes.Status422UnprocessableEntity);
            })
            .AddMongo(builder.Configuration, healthCheckBuilder)
            .AddAuthentication(builder.Configuration)
            .AddJobs(builder.Configuration, healthCheckBuilder)
            .AddHttpClients(builder.Configuration)
            .AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining<CreateReportScheduleHandler>();
                options.RegisterServicesFromAssemblyContaining<GetReportScheduleByIdQueryHandler>();
                options.AddOpenRequestPostProcessor(typeof(CheckPermissionsOnOwnedQueryResourceProcessor<,>),
                    ServiceLifetime.Scoped);
                options.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>), ServiceLifetime.Scoped);
                options.Lifetime = ServiceLifetime.Scoped;
            })
            .AddValidatorsFromAssemblyContaining<ReportScheduleValidator>()
            .AddValidatorsFromAssemblyContaining<PatchReportScheduleValidator>()
            .AddReportSchedulesFeature()
            .AddReportTemplatesFeature()
            .AddReportExecutionsFeature();
    }

    private static void Configure(WebApplication app)
    {
        app.UseForwardedHeaders();

        app.MapHealthChecks("/health/readiness",
            new HealthCheckOptions
            {
                Predicate = hc => hc.Tags.Contains(HealthCheckConstants.OnReadinessTag),
                ResponseWriter = HealthCheckResponseWriterDelegates.WriteReadinessResponse
            });

        app.MapHealthChecks("/health/liveness",
            new HealthCheckOptions
            {
                Predicate = _ => false, // Does not run any checks, just returns whether the app is up or not
                ResponseWriter = HealthCheckResponseWriterDelegates.WriteLivenessResponse
            });

        app
            .UseHttpsRedirection()
            .UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".yaml"] = "application/x-yaml", [".yml"] = "application/x-yaml" }
                }
            })
            .UseProblemDetails()
            .UseDocumentationServices()
            .UseAuthentication()
            .UseAuthorization();

        var hangfireSettings = app.Services.GetRequiredService<IOptions<HangfireSettings>>();

        if (hangfireSettings.Value.Dashboard.Enabled)
        {
            app.MapHangfireDashboardWithAuthorizationPolicy(AuthConstants.HangfirePolicyName,
                options: new DashboardOptions
                {
                    DarkModeEnabled = true,
                    DisplayStorageConnectionString = hangfireSettings.Value.DisplayStorageConnectionString,
                    AppPath = null
                });
        }

        app.MapControllers();
    }
}
