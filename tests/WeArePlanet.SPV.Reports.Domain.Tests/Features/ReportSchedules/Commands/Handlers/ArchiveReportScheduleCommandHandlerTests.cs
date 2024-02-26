using MediatR;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Handlers;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Commands.Handlers;

public class ArchiveReportScheduleCommandHandlerTests : PlanetTestClass
{
    private readonly IMediator mediatorMock;
    private readonly IReportScheduleRepository repositoryMock;
    private readonly ArchiveReportScheduleCommandHandler sut;

    public ArchiveReportScheduleCommandHandlerTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        this.mediatorMock = Substitute.For<IMediator>();
        this.repositoryMock = Substitute.For<IReportScheduleRepository>();

        this.sut = new ArchiveReportScheduleCommandHandler(this.mediatorMock, this.repositoryMock,
            StaticDateTimeProvider.Instance);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsConfiguration()
    {
        // Arrange
        var projection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        var request = new ArchiveReportScheduleCommand(projection.Id);
        var cancellationToken = new CancellationToken();

        this.repositoryMock.AddOrUpdateAsync(Arg.Any<ReportSchedule>(), cancellationToken)
            .Returns(Task.CompletedTask);

        this.mediatorMock
            .Send(Arg.Is<GetReportScheduleByIdQuery>(req => req.Id == request.Id), cancellationToken)
            .Returns(projection);

        // Act
        var handlerResponse = await this.sut.Handle(request, cancellationToken);

        // Assert
        handlerResponse.Should().NotBeNull();
    }
}
