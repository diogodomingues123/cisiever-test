using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Handlers;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Queries.Handlers;

public class GetReportScheduleByIdQueryHandlerTests : PlanetTestClass
{
    private readonly IReportScheduleRepository repositoryMock;
    private readonly GetReportScheduleByIdQueryHandler sut;

    public GetReportScheduleByIdQueryHandlerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.repositoryMock = Substitute.For<IReportScheduleRepository>();
        this.sut = new GetReportScheduleByIdQueryHandler(this.repositoryMock);
    }

    [Fact]
    public async Task Handle_ExistingReportSchedule_ReturnsSuccessfully()
    {
        // Arrange
        var query = new GetReportScheduleByIdQuery(ReportScheduleId.Create());
        var scheduleProjection = this.ObjectFactoryRegistry.Generate<ReportScheduleProjection>();

        this.repositoryMock
            .GetByIdAsync(query.Id)
            .Returns(scheduleProjection);

        // Act
        var result = await this.sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(scheduleProjection);
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetReportScheduleByIdQuery(ReportScheduleId.Create());

        this.repositoryMock
            .GetByIdAsync(query.Id)
            .Returns((ReportScheduleProjection)default);

        // Act
        var action = () => this.sut.Handle(query, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
