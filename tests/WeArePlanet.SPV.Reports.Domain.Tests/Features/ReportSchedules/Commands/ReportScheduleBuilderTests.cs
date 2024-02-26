using FluentAssertions.Extensions;

using FluentValidation;
using FluentValidation.Results;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands;

public class ReportScheduleBuilderTests : PlanetTestClass
{
    private readonly IDateTimeProvider dateTimeProviderMock;
    private readonly IReportTemplateConfigurationsRepository reportTemplateRepositoryMock;

    private readonly ReportScheduleBuilder sut;
    private readonly IValidator<ReportSchedule> validatorMock;

    public ReportScheduleBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.validatorMock = Substitute.For<IValidator<ReportSchedule>>();
        this.reportTemplateRepositoryMock = Substitute.For<IReportTemplateConfigurationsRepository>();
        this.dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        this.dateTimeProviderMock
            .GetUtcNow()
            .Returns(16.October(2021).At(20.Hours().And(25.Minutes())));

        this.validatorMock
            .ValidateAsync(Arg.Any<ReportSchedule>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        this.sut = new ReportScheduleBuilder(this.validatorMock, this.reportTemplateRepositoryMock,
            this.dateTimeProviderMock);
    }

    [Fact]
    public async Task BuildAsync_NewInstance_CreatesProperInstance()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlan);

        this.reportTemplateRepositoryMock
            .GetAsync(Arg.Is(instance.Template.Id), Arg.Any<CancellationToken>())
            .Returns(instance.Template);

        // Act
        var result = await this.sut
            .WithName(instance.Name)
            .WithExecutionPlan(instance.ExecutionPlan)
            .WithInput(instance.Input)
            .WithOwner(instance.Owner)
            .WithWebhooks(instance.WebhookConfiguration)
            .UsingTemplateConfiguration(instance.Template.Id)
            .IsActive(true)
            .BuildAsync();

        // Assert
        result.IsRight.Should().BeTrue();
        result.IfRight(schedule =>
        {
            schedule.Should().BeEquivalentTo(instance, options => options
                .ComparingByMembers<ReportSchedule>()
                .Excluding(s => s.Id)
                .Excluding(s => s.CreatedAt)
                .Excluding(s => s.UpdatedAt)
                .Excluding(s => s.DomainEvents));
        });
    }

    [Fact]
    public async Task BuildAsync_NewInstanceWithExisting_ReplacesAllFields_BuildsCorrectInstance()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlan);
        var existingProjection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        this.reportTemplateRepositoryMock
            .GetAsync(Arg.Is(instance.Template.Id), Arg.Any<CancellationToken>())
            .Returns(instance.Template);

        var expectedUpdateDate = 20.October(2022).At(20.Hours().And(25.Minutes())).WithOffset(1.Hours());

        this.dateTimeProviderMock.GetUtcNow()
            .Returns(expectedUpdateDate);

        // Act
        var result = await this.sut
            .FromExisting(existingProjection)
            .WithName(instance.Name)
            .WithExecutionPlan(instance.ExecutionPlan)
            .WithInput(instance.Input)
            .WithOwner(instance.Owner)
            .WithWebhooks(instance.WebhookConfiguration)
            .UsingTemplateConfiguration(instance.Template.Id)
            .IsActive(true)
            .BuildAsync();

        // Assert
        result.IsRight.Should().BeTrue();
        result.IfRight(schedule =>
        {
            schedule.Should().BeEquivalentTo(instance, options => options
                .ComparingByMembers<ReportSchedule>()
                .Excluding(s => s.Id)
                .Excluding(s => s.Owner)
                .Excluding(s => s.CreatedAt)
                .Excluding(s => s.DomainEvents)
                .Excluding(s => s.UpdatedAt)
            );

            // The Owner should stick the same even if overriden in the builder
            schedule.Owner.Should().BeEquivalentTo(existingProjection.Owner);

            // The CreatedAt value should never change if there is an existing representation in the database
            schedule.CreatedAt.Should().Be(existingProjection.CreatedAt);

            // The UpdatedAt value should change to the expected one
            schedule.UpdatedAt.Should().Be(expectedUpdateDate.UtcDateTime);
        });
    }

    [Fact]
    public async Task BuildAsync_NewInstanceWithExisting_ReplacesEnabled_BuildsCorrectInstance()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlan);
        var existingProjection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        this.reportTemplateRepositoryMock
            .GetAsync(Arg.Is(instance.Template.Id), Arg.Any<CancellationToken>())
            .Returns(instance.Template);

        IReportScheduleState expectedState = existingProjection.State is ActiveReportScheduleState
            ? new InactiveReportScheduleState()
            : new ActiveReportScheduleState();
        var toggleToSet = existingProjection.State is not ActiveReportScheduleState;

        // Act
        var result = await this.sut
            .FromExisting(existingProjection)
            .IsActive(toggleToSet)
            .BuildAsync();

        // Assert
        result.IsRight.Should().BeTrue();
        result.IfRight(schedule =>
        {
            schedule.Should().BeEquivalentTo(existingProjection, options => options
                .Excluding(rs => rs.UpdatedAt)
                .Excluding(rs => rs.State));

            // The IsEnabled should be the only changed field
            schedule.IsActive.Should().Be(toggleToSet);
            schedule.State.Should().BeOfType(expectedState.GetType());
        });
    }

    [Fact]
    public async Task BuildAsync_InvalidInstance_ReturnsValidationResult()
    {
        // Arrange
        var instance =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlan);

        this.reportTemplateRepositoryMock
            .GetAsync(Arg.Is(instance.Template.Id), Arg.Any<CancellationToken>())
            .Returns(instance.Template);

        this.validatorMock
            .ValidateAsync(Arg.Any<ReportSchedule>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("abc", "cdf") }));

        // Act
        var result = await this.sut
            .WithName(instance.Name)
            .WithExecutionPlan(instance.ExecutionPlan)
            .WithInput(instance.Input)
            .WithOwner(instance.Owner)
            .WithWebhooks(instance.WebhookConfiguration)
            .UsingTemplateConfiguration(instance.Template.Id)
            .BuildAsync();

        // Assert
        result.IsLeft.Should().BeTrue();
    }
}
