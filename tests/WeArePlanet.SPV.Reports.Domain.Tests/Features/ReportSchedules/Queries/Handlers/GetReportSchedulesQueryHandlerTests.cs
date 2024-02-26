using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Auth;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Handlers;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Features.ReportSchedules.Queries.Handlers;

public class GetReportSchedulesQueryHandlerTests : PlanetTestClass
{
    private readonly IAuthenticationPrincipal authenticationPrincipalMock;
    private readonly IReportScheduleRepository repositoryMock;

    private readonly GetReportSchedulesQueryHandler sut;

    public GetReportSchedulesQueryHandlerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.repositoryMock = Substitute.For<IReportScheduleRepository>();
        this.authenticationPrincipalMock = Substitute.For<IAuthenticationPrincipal>();

        this.sut = new GetReportSchedulesQueryHandler(this.repositoryMock, this.authenticationPrincipalMock);
    }

    [Fact]
    public async Task Handle_ExistingReportSchedule_ReturnsSuccessfully()
    {
        // Arrange
        var query = new GetReportSchedulesQuery(1, 2);
        var items = this.ObjectFactoryRegistry.GenerateMany<ReportScheduleProjection>().ToList();
        var scheduleProjectionPage = new Page<ReportScheduleProjection>(items, 1, 2, 3);
        var cancellationToken = new CancellationToken();

        this.repositoryMock
            .GetAllAsync(query, this.authenticationPrincipalMock, cancellationToken)
            .Returns(scheduleProjectionPage);

        // Act
        var result = await this.sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(scheduleProjectionPage);
    }
}
