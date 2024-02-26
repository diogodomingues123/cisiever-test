using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Mappers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Commands.Mappers;

public class ReportConfigurationInputMapperExtensionsTests : PlanetTestClass
{
    public ReportConfigurationInputMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToDomainReportConfigurationInput_UsingJObject_MapsSuccessfully()
    {
        // Arrange
        var jobject = JsonConvert.DeserializeObject<JObject>(
            """
            {
              "field1": "a",
              "field2": 1
            }
            """);

        var expected = new ReportScheduleInput(jobject!.ToObject<IDictionary<string, object>>()!);

        // Act
        var result = jobject!.ToDomainReportScheduleInput();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToDomainReportConfigurationInput_UsingDictionary_MapsSuccessfully()
    {
        // Arrange
        var dictionary = JsonConvert.DeserializeObject<JObject>(
            """
            {
              "field1": "a",
              "field2": 1
            }
            """)?.ToObject<IDictionary<string, object>>();

        var expected = new ReportScheduleInput(dictionary!);

        // Act
        var result = dictionary!.ToDomainReportScheduleInput();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
