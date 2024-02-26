using Bogus;

using FluentValidation;
using FluentValidation.TestHelper;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Validators;

public class ReportScheduleInputValidatorTests : PlanetTestClass
{
    private readonly ReportScheduleValidator reportScheduleValidator;
    private readonly ReportScheduleInputValidator sut;

    public ReportScheduleInputValidatorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new ReportScheduleInputValidator();
        this.reportScheduleValidator = new ReportScheduleValidator(
            this.sut,
            Substitute.For<IValidator<OneOffReportExecutionPlan>>(),
            Substitute.For<IValidator<RecurringReportExecutionPlan>>());
    }

    [Fact]
    public async Task ValidateAsync_ValidSchedule_ReturnsValidValidationResult()
    {
        // Arrange
        var faker = new Faker();
        var template = new ReportTemplateConfiguration(
            Guid.NewGuid(),
            faker.Random.Hash(),
            faker.Random.Hash(),
            faker.Random.Enum<ReportTemplateFormat>(),
            faker.Random.Hash()
        );

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field1", true, new StringFieldType()));

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field2", false, new StringFieldType()));

        var input = new ReportScheduleInput(new Dictionary<string, object> { { "field1", "2" } });
        var reportConfiguration = new ReportSchedule(
            ReportScheduleId.Create(),
            faker.Random.Hash(),
            this.ObjectFactoryRegistry.Generate<Owner>(),
            template,
            this.ObjectFactoryRegistry.Generate<IReportExecutionPlan>(),
            new ActiveReportScheduleState(),
            input
        );

        // Act
        var result = await this.reportScheduleValidator.TestValidateAsync(reportConfiguration);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Input);
    }

    [Fact]
    public async Task ValidateAsync_ValidSchedule_2RequiredFields_ReturnsValidValidationResult()
    {
        // Arrange
        var faker = new Faker();
        var template = new ReportTemplateConfiguration(
            Guid.NewGuid(),
            faker.Random.Hash(),
            faker.Random.Hash(),
            faker.Random.Enum<ReportTemplateFormat>(),
            faker.Random.Hash()
        );

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field1", true, new StringFieldType()));

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field2", true, new DateTimeFieldType()));

        var input = new ReportScheduleInput(new Dictionary<string, object>
        {
            { "field1", "2" }, { "field2", DateTime.UtcNow }
        });

        var reportConfiguration = new ReportSchedule(
            ReportScheduleId.Create(),
            faker.Random.Hash(),
            this.ObjectFactoryRegistry.Generate<Owner>(),
            template,
            this.ObjectFactoryRegistry.Generate<IReportExecutionPlan>(),
            new ActiveReportScheduleState(),
            input
        );

        // Act
        var result = await this.reportScheduleValidator.TestValidateAsync(reportConfiguration);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Input);
    }

    [Fact]
    public async Task ValidateAsync_Invalid_MissingRequiredField_ReturnsInvalidValidationResult()
    {
        // Arrange
        var faker = new Faker();
        var template = new ReportTemplateConfiguration(
            Guid.NewGuid(),
            faker.Random.Hash(),
            faker.Random.Hash(),
            faker.Random.Enum<ReportTemplateFormat>(),
            faker.Random.Hash()
        );

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field1", true, new StringFieldType()));

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field2", true, new DateTimeFieldType()));

        var input = new ReportScheduleInput(new Dictionary<string, object>
        {
            { "field1", "2" } // missing field2
        });

        var reportConfiguration = new ReportSchedule(
            ReportScheduleId.Create(),
            faker.Random.Hash(),
            this.ObjectFactoryRegistry.Generate<Owner>(),
            template,
            this.ObjectFactoryRegistry.Generate<IReportExecutionPlan>(),
            new ActiveReportScheduleState(),
            input
        );

        // Act
        var result = await this.reportScheduleValidator.TestValidateAsync(reportConfiguration);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Input);
    }

    [Fact]
    public async Task ValidateAsync_Invalid_FieldNotOfType_ReturnsInvalidValidationResult()
    {
        // Arrange
        var faker = new Faker();
        var template = new ReportTemplateConfiguration(
            Guid.NewGuid(),
            faker.Random.Hash(),
            faker.Random.Hash(),
            faker.Random.Enum<ReportTemplateFormat>(),
            faker.Random.Hash()
        );

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field1", true, new StringFieldType()));

        template.AddInputFieldConfiguration(new InputFieldConfiguration(
            "field2", true, new DateTimeFieldType()));

        var input = new ReportScheduleInput(new Dictionary<string, object>
        {
            { "field1", "2" }, { "field2", 2 } // wrong type for field2
        });

        var reportConfiguration = new ReportSchedule(
            ReportScheduleId.Create(),
            faker.Random.Hash(),
            this.ObjectFactoryRegistry.Generate<Owner>(),
            template,
            this.ObjectFactoryRegistry.Generate<IReportExecutionPlan>(),
            new ActiveReportScheduleState(),
            input
        );

        // Act
        var result = await this.reportScheduleValidator.TestValidateAsync(reportConfiguration);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Input);
    }
}
