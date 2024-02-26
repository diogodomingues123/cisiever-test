using FluentAssertions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;
using WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

using Xunit.Abstractions;

namespace WeArePlanet.SPV.Reports.Persistence.Tests.ReportSchedules.Mappers;

public class ReportScheduleMapperExtensionsTests : PlanetTestClass
{
    public ReportScheduleMapperExtensionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void ToMongoDocument_ValidInstance_MapsSuccessfully()
    {
        // Arrange
        var reportSchedule =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlan);
        var expected = new ReportScheduleDocument
        {
            Id = reportSchedule.Id!.Value.ToString(),
            Name = reportSchedule.Name,
            Owner =
                new OwnerDocument
                {
                    UserId = reportSchedule.Owner.UserId, OrganizationId = reportSchedule.Owner.OrganizationId
                },
            ExecutionPlan =
                new ExecutionPlanDocument
                {
                    Id = reportSchedule.ExecutionPlan.Id!.Value!,
                    Type = ScheduleDocumentType.OneOff,
                    Date = ((OneOffReportExecutionPlan)reportSchedule.ExecutionPlan).Date,
                    TimeZoneId = TimeZoneId.Utc.Value
                },
            Input = new Dictionary<string, object>(reportSchedule.Input!.Parameters),
            WebhookConfiguration = new WebhookConfigurationDocument
            {
                OnSuccess = reportSchedule.WebhookConfiguration!.OnSuccess,
                OnFailure = reportSchedule.WebhookConfiguration.OnFailure
            },
            CreatedAt = reportSchedule.CreatedAt,
            UpdatedAt = reportSchedule.UpdatedAt,
            TemplateId = reportSchedule.Template.Id.ToString(),
            State = reportSchedule.State.ToString()!
        };

        // Act
        var result = reportSchedule.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(s => s.MongoId));
    }

    [Fact]
    public void ToMongoDocument_NoInput_MapsSuccessfully()
    {
        // Arrange
        var reportSchedule =
            this.ObjectFactoryRegistry.Generate<ReportSchedule>(ReportScheduleFactory.ObjectNames
                .ReportScheduleWithOneOffExecutionPlanAndNoInput);

        var expected = new ReportScheduleDocument
        {
            Id = reportSchedule.Id!.Value.ToString(),
            Name = reportSchedule.Name,
            Owner =
                new OwnerDocument
                {
                    UserId = reportSchedule.Owner.UserId, OrganizationId = reportSchedule.Owner.OrganizationId
                },
            ExecutionPlan =
                new ExecutionPlanDocument
                {
                    Id = reportSchedule.ExecutionPlan.Id!.Value!,
                    Type = ScheduleDocumentType.OneOff,
                    Date = ((OneOffReportExecutionPlan)reportSchedule.ExecutionPlan).Date
                },
            Input = null,
            TemplateId = reportSchedule.Template.Id.ToString()
        };

        // Act
        var result = reportSchedule.ToMongoDocument();

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Including(s => s.Input));
    }

    [Fact]
    public void ToMongoDocument_NullInstance_ThrowsArgumentNullException()
    {
        // Arrange
        ReportSchedule? reportSchedule = null;

        // Act
        var result = () => reportSchedule!.ToMongoDocument();

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }
}
