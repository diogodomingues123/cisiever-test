using System.Globalization;
using System.Reflection;

using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;

namespace WeArePlanet.SPV.Reports.Common.Hangfire;

public class InjectParametersJobFilter : JobFilterAttribute, IClientFilter
{
    public void OnCreating(CreatingContext context)
    {
        var jobArgs = context.Job.Args
            .Select((arg, pos) => (Value: arg, Position: pos))
            .Where(argTuple => argTuple.Value is not null)
            .Where(argTuple => argTuple.Value is not CancellationToken and not PerformContext)
            .ToList();

        foreach (var (value, position) in jobArgs)
        {
            var propertyName = GetParameterName(context.Job.Method.GetParameters()[position]);

            context.SetJobParameter(propertyName,
                value.ToString());
        }
    }

    public void OnCreated(CreatedContext filterContext)
    {
    }

    private static string GetParameterName(ParameterInfo jobParameterInfo)
    {
        var jobParameterNameAttribute =
            (JobParameterNameAttribute?)jobParameterInfo.GetCustomAttribute(typeof(JobParameterNameAttribute));

        return jobParameterNameAttribute?.Name ??
               CultureInfo.CurrentCulture.TextInfo.ToTitleCase(jobParameterInfo.Name!);
    }
}
