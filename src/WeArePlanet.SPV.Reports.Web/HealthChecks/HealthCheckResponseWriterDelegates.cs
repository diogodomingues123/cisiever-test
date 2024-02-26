using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace WeArePlanet.SPV.Reports.Web.HealthChecks;

[ExcludeFromCodeCoverage]
public static class HealthCheckResponseWriterDelegates
{
    public static async Task WriteReadinessResponse(HttpContext httpContext, HealthReport healthReport)
    {
        var jsonOptions = httpContext.RequestServices.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>().Value;

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var response = new HealthCheckReadinessResponse
        {
            Status = healthReport.Status,
            Checks = healthReport.Entries.Select(entry => new HealthCheckEntry
            {
                Name = entry.Key,
                Status = entry.Value.Status,
                Duration = entry.Value.Duration,
                Tags = entry.Value.Tags.ToImmutableList(),
                Data = entry.Value.Data.ToImmutableDictionary()
            }).ToImmutableList()
        };

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response, jsonOptions.SerializerSettings));
    }

    public static async Task WriteLivenessResponse(HttpContext httpContext, HealthReport healthReport)
    {
        await httpContext.Response.WriteAsync(string.Empty);
    }
}
