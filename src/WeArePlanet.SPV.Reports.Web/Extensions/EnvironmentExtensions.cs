namespace WeArePlanet.SPV.Reports.Web.Extensions;

public static class EnvironmentExtensions
{
    public static bool IsLocal(this IWebHostEnvironment environment)
    {
        return environment.IsEnvironment("Local");
    }
}
