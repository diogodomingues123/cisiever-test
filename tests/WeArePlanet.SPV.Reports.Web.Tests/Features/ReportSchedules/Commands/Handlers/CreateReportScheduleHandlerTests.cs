using FluentValidation.Results;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Commands.Handlers;

public class CreateReportScheduleHandlerTests : PlanetTestClass
{
    private readonly IReportScheduleBuilder builderMock;
    private readonly IAuthenticationPrincipal principalMock;
    private readonly IReportScheduleRepository repositoryMock;
    private readonly CreateReportScheduleHandler sut;

    public CreateReportScheduleHandlerTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        this.builderMock = Substitute.For<IReportScheduleBuilder>();
        this.principalMock = Substitute.For<IAuthenticationPrincipal>();
        this.repositoryMock = Substitute.For<IReportScheduleRepository>();

        this.sut = new CreateReportScheduleHandler(this.builderMock, this.principalMock, this.repositoryMock,
            StaticDateTimeProvider.Instance);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsConfiguration()
    {
        // Arrange
        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();

        var request = this.ObjectFactoryRegistry.Generate<CreateReportScheduleRequestContract>();
        var cancellationToken = new CancellationToken();
        this.builderMock.BuildAsync(cancellationToken)
            .Returns(configuration);

        this.repositoryMock.AddOrUpdateAsync(configuration, cancellationToken)
            .Returns(Task.CompletedTask);

        this.principalMock.UserId.Returns("abc");
        this.principalMock.OrganizationId.Returns("abc2");

        // Act
        var handlerResponse = await this.sut.Handle(request, cancellationToken);

        // Assert
        handlerResponse.IsRight.Should().BeTrue();
        handlerResponse.IfRight(r => r.Id.Should().NotBeEmpty());
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationResult()
    {
        // Arrange
        var request = this.ObjectFactoryRegistry.Generate<CreateReportScheduleRequestContract>();
        var cancellationToken = new CancellationToken();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("a", "b") });
        this.builderMock.BuildAsync(cancellationToken)
            .Returns(validationResult);

        this.principalMock.UserId.Returns("abc");
        this.principalMock.OrganizationId.Returns("abc2");

        // Act
        var handlerResponse = await this.sut.Handle(request, cancellationToken);

        // Assert
        handlerResponse.IsLeft.Should().BeTrue();
        handlerResponse.IfLeft(result => result.Should().Be(validationResult));
    }
}
