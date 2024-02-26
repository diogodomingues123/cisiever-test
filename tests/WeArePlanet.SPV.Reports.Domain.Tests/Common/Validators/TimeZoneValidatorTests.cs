using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Validators;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Common.Validators;

public class TimeZoneValidatorTests : PlanetTestClass
{
    private readonly TimeZoneValidator sut;

    public TimeZoneValidatorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new TimeZoneValidator();
    }

    [Theory]
    [InlineData("UTC")]
    [InlineData("Europe/Lisbon")]
    [InlineData("Pacific/Honolulu")]
    [InlineData("America/New_York")]
    public void Validate_ValidTimeZones_ReturnsValid(string timeZoneId)
    {
        // Arrange
        var timeZone = new TimeZoneId(timeZoneId);

        // Act
        var result = this.sut.Validate(timeZone);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("Europe/Porto")]
    [InlineData("Pacific/New_York")]
    [InlineData(null)]
    public void Validate_InvalidTimeZones_ReturnsInvalid(string timeZoneId)
    {
        // Arrange
        var timeZone = new TimeZoneId(timeZoneId);

        // Act
        var result = this.sut.Validate(timeZone);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
