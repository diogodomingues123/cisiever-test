namespace WeArePlanet.SPV.Reports.Common.Hangfire;

[AttributeUsage(AttributeTargets.Parameter)]
public class JobParameterNameAttribute : Attribute
{
    public JobParameterNameAttribute(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}
