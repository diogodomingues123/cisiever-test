using FluentValidation;
using FluentValidation.TestHelper;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Validators;

public class RecurringReportExecutionPlanValidatorTests : PlanetTestClass
{
    private readonly RecurringReportExecutionPlanValidator sut;
    private readonly IValidator<TimeZoneId> timeZoneValidatorMock;

    public RecurringReportExecutionPlanValidatorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.timeZoneValidatorMock = Substitute.For<IValidator<TimeZoneId>>();

        this.sut = new RecurringReportExecutionPlanValidator(this.timeZoneValidatorMock);
    }

    [Theory]
    [InlineData("* * * * *")]
    [InlineData("0 12 * */2 Mon")]
    [InlineData("1/2 0 * * 1")]
    public async Task ValidateAsync_ValidSchedule_ReturnsValidValidationResult(string frequency)
    {
        // Arrange
        var recurringReportSchedule = new RecurringReportExecutionPlan(frequency, null);

        // Act
        var result = await this.sut.ValidateAsync(recurringReportSchedule);

        // Assert
        result.IsValid.Should().BeTrue(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));
    }

    [Theory]
    [InlineData("dqwdqw")]
    [InlineData("1312 12 222 222 1")]
    [InlineData("22 11 11 11 11")]
    public async Task ValidateAsync_InvalidSchedule_ReturnsInvalidValidationResult(string frequency)
    {
        // Arrange
        var recurringReportSchedule = new RecurringReportExecutionPlan(frequency, null);

        // Act
        var result = await this.sut.TestValidateAsync(recurringReportSchedule);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(e => e.Frequency);
    }
}
