using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Controllers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Controllers;

public class ReportSchedulesControllerTests : PlanetTestClass
{
    private readonly IMediator mediatorMock;
    private readonly ReportSchedulesController sut;

    public ReportSchedulesControllerTests(ITestOutputHelper helper)
        : base(helper)
    {
        this.mediatorMock = Substitute.For<IMediator>();
        this.sut = new ReportSchedulesController(this.mediatorMock, StaticDateTimeProvider.Instance);
    }

    [Fact]
    public async Task CreateReport_WhenSuccessfulCreation_ReturnsCreatedAt()
    {
        // Arrange
        var body = this.ObjectFactoryRegistry.Generate<CreateReportScheduleRequestContract>();
        var cancellationToken = new CancellationToken();
        var configId = Guid.NewGuid();
        var expectedResponse = new ReportScheduleContract(
            DateTime.UtcNow,
            body.ExecutionPlan,
            configId,
            body.Input,
            body.Name,
            ReportScheduleContractState.Active,
            body.TemplateId,
            DateTime.UtcNow,
            body.Webhooks);

        this.mediatorMock
            .Send(Arg.Is(body), cancellationToken)
            .Returns(expectedResponse);

        // Act
        var actionResult = await this.sut.CreateReportSchedule(body, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<CreatedAtActionResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task CreateReport_WhenFailedCreation_ReturnsUnprocessableEntity()
    {
        // Arrange
        var body = this.ObjectFactoryRegistry.Generate<CreateReportScheduleRequestContract>();
        var cancellationToken = new CancellationToken();
        var expectedResponse = new ValidationResult { Errors = new List<ValidationFailure> { new("a", "abc") } };

        this.mediatorMock
            .Send(Arg.Is(body), cancellationToken)
            .Returns(expectedResponse);

        // Act
        var actionResult = await this.sut.CreateReportSchedule(body, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<UnprocessableEntityObjectResult>();
    }

    [Fact]
    public async Task GetReportById_WhenExisting_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var scheduleProjection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();
        var cancellationToken = new CancellationToken();

        this.mediatorMock
            .Send(Arg.Is(new GetReportScheduleByIdQuery(new ReportScheduleId(id))), cancellationToken)
            .Returns(scheduleProjection);

        // Act
        var actionResult = await this.sut.GetReportSchedule(id, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<ReportScheduleContract>()
            .Which.Id.Should().Be(scheduleProjection.Id.Value);
    }

    [Fact]
    public async Task PatchReport_WhenSuccessfulPatching_ReturnsOk()
    {
        // Arrange
        var body = this.ObjectFactoryRegistry.Generate<PatchReportScheduleRequest>();
        var cancellationToken = new CancellationToken();
        var expectedResponse = this.ObjectFactoryRegistry.Generate<ReportScheduleContract>();

        this.mediatorMock
            .Send(Arg.Is(body), cancellationToken)
            .Returns(expectedResponse);

        // Act
        var actionResult = await this.sut.PatchReportSchedule(body.Id!.Value, body, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task PatchReport_WhenFailedPatching_ReturnsUnprocessableEntity()
    {
        // Arrange
        var body = this.ObjectFactoryRegistry.Generate<PatchReportScheduleRequest>();
        var cancellationToken = new CancellationToken();
        var expectedResponse = new ValidationResult { Errors = new List<ValidationFailure> { new("a", "abc") } };

        this.mediatorMock
            .Send(Arg.Is(body), cancellationToken)
            .Returns(expectedResponse);

        // Act
        var actionResult = await this.sut.PatchReportSchedule(body.Id!.Value, body, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<UnprocessableEntityObjectResult>();
    }

    [Fact]
    public async Task ArchiveReport_WhenExisting_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        this.mediatorMock
            .Send(Arg.Is(new ArchiveReportScheduleCommand(new ReportScheduleId(id))), cancellationToken)
            .Returns(Unit.Value);

        // Act
        var actionResult = await this.sut.ArchiveReportSchedule(id, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetReportSchedules_ReturnsOk()
    {
        // Arrange
        var request = (PageNumber: 1, PageSize: 30);
        var cancellationToken = new CancellationToken();
        var entries = this.ObjectFactoryRegistry.GenerateMany<ReportScheduleProjection>().ToList();
        var returnedPage = new Page<ReportScheduleProjection>(
            entries, request.PageNumber,
            request.PageSize,
            10000);

        this.mediatorMock
            .Send(Arg.Is(new GetReportSchedulesQuery(request.PageNumber, request.PageSize)), cancellationToken)
            .Returns(returnedPage);

        // Act
        var actionResult = await this.sut.GetReportSchedules(request.PageNumber, request.PageSize, cancellationToken);

        // Assert
        actionResult.Should().NotBeNull();
        var receivedPage = actionResult.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<ReportSchedulePageContract>()
            .Which;

        receivedPage.PageNumber.Should().Be(returnedPage.PageNumber);
        receivedPage.TotalItems.Should().Be((int)returnedPage.TotalItems);
        receivedPage.Entries.Should().HaveCount(entries.Count);
    }
}
