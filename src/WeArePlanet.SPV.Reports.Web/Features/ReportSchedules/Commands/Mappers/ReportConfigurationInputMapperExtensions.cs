using Newtonsoft.Json.Linq;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

public static class ReportConfigurationInputMapperExtensions
{
    public static ReportScheduleInput? ToDomainReportScheduleInput(this object input)
    {
        return input switch
        {
            IDictionary<string, object> dictionary => new ReportScheduleInput(dictionary),
            JObject jObject => new ReportScheduleInput(jObject.ToObject<Dictionary<string, object>>() ??
                                                       throw new InvalidOperationException(
                                                           "Invalid JObject instance.")),
            _ => null
        };
    }

    public static object? ToContractReportConfigurationInput(this ReportScheduleInput input)
    {
        return input.Parameters;
    }
}
