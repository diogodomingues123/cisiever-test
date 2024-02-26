using MediatR;

using NSubstitute.ExceptionExtensions;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.TestFactories.Common;
using WeArePlanet.SPV.Reports.Web.Contracts;
using WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.Commands.Handlers;

namespace WeArePlanet.SPV.Reports.Web.Tests.Features.ReportSchedules.Commands.Handlers;

public class PatchReportScheduleHandlerTests : PlanetTestClass
{
    private readonly IReportScheduleBuilder builderMock;
    private readonly IMediator mediatorMock;
    private readonly IReportScheduleRepository repositoryMock;
    private readonly PatchReportScheduleHandler sut;

    public PatchReportScheduleHandlerTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        this.builderMock = Substitute.For<IReportScheduleBuilder>();
        this.mediatorMock = Substitute.For<IMediator>();
        this.repositoryMock = Substitute.For<IReportScheduleRepository>();

        this.sut = new PatchReportScheduleHandler(this.builderMock, this.repositoryMock,
            StaticDateTimeProvider.Instance, this.mediatorMock);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsConfiguration()
    {
        // Arrange
        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();
        var projection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        var request = new PatchReportScheduleRequest(true) { Id = projection.Id };

        var cancellationToken = new CancellationToken();
        this.builderMock.BuildAsync(cancellationToken)
            .Returns(configuration);

        this.repositoryMock.AddOrUpdateAsync(configuration, cancellationToken)
            .Returns(Task.CompletedTask);

        this.mediatorMock
            .Send(Arg.Is<GetReportScheduleByIdQuery>(req => req.Id == request.Id), cancellationToken)
            .Returns(projection);

        // Act
        var handlerResponse = await this.sut.Handle(request, cancellationToken);

        // Assert
        handlerResponse.IsRight.Should().BeTrue();
        handlerResponse.IfRight(r =>
        {
            r.Id.Should().Be(configuration.Id!.Value);
            r.State!.Value.Should().Be(ReportScheduleContractState.Active);
        });
    }

    [Fact]
    public async Task Handle_EmptyRequest_DoesNotCauseSideEffects_ReturnsConfiguration()
    {
        // Arrange
        var configuration = this.ObjectFactoryRegistry.Generate<ReportSchedule>();
        var projection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        var request = new PatchReportScheduleRequest(null) { Id = projection.Id };

        var cancellationToken = new CancellationToken();
        this.builderMock.BuildAsync(cancellationToken)
            .Returns(configuration);

        this.mediatorMock
            .Send(Arg.Is<GetReportScheduleByIdQuery>(req => req.Id == request.Id), cancellationToken)
            .Returns(projection);

        this.repositoryMock.AddOrUpdateAsync(configuration, cancellationToken)
            .ThrowsForAnyArgs(new Exception("Repository should not be called."));

        // Act
        var handlerResponse = await this.sut.Handle(request, cancellationToken);

        // Assert
        handlerResponse.IsRight.Should().BeTrue();
        handlerResponse.IfRight(r =>
        {
            r.Id.Should().Be(request.Id!.Value);
            r.State!.Should().Be(ReportScheduleContractState.Active);
        });
    }
}
