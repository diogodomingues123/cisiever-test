using FluentAssertions.Extensions;

using FluentValidation;
using FluentValidation.TestHelper;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Validators;

public class OneOffReportExecutionPlanValidatorTests : PlanetTestClass
{
    private readonly IDateTimeProvider dateTimeProviderMock;
    private readonly OneOffReportExecutionPlanValidator sut;
    private readonly IValidator<TimeZoneId> timeZoneValidatorMock;

    public OneOffReportExecutionPlanValidatorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        this.timeZoneValidatorMock = Substitute.For<IValidator<TimeZoneId>>();

        this.sut = new OneOffReportExecutionPlanValidator(this.dateTimeProviderMock, this.timeZoneValidatorMock);
    }

    [Fact]
    public async Task ValidateAsync_ValidSchedule_ReturnsValidValidationResult()
    {
        // Arrange
        var now = 15.October(2023).At(15.Hours());
        this.dateTimeProviderMock.GetUtcNow()
            .Returns(now);

        var oneOffReportSchedule = new OneOffReportExecutionPlan(now + 1.Hours(), null);

        // Act
        var result = await this.sut.ValidateAsync(oneOffReportSchedule);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_PastDate_ReturnsInvalidValidationResult()
    {
        // Arrange
        var now = 15.October(2023).At(15.Hours());
        this.dateTimeProviderMock.GetUtcNow()
            .Returns(now);

        // Schedule in the past
        var oneOffReportSchedule = new OneOffReportExecutionPlan(now - 1.Hours(), null);

        // Act
        var result = await this.sut.TestValidateAsync(oneOffReportSchedule);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }

    [Fact]
    public async Task ValidateAsync_MinDate_ReturnsInvalidValidationResult()
    {
        // Arrange
        var now = DateTimeOffset.MinValue.UtcDateTime;

        // Schedule to the min value
        var oneOffReportSchedule = new OneOffReportExecutionPlan(now, null);

        // Act
        var result = await this.sut.TestValidateAsync(oneOffReportSchedule);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }

    [Fact]
    public async Task ValidateAsync_NoDate_ReturnsValidValidationResult()
    {
        // Arrange
        var oneOffReportSchedule = new OneOffReportExecutionPlan();

        // Act
        var result = await this.sut.TestValidateAsync(oneOffReportSchedule);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
