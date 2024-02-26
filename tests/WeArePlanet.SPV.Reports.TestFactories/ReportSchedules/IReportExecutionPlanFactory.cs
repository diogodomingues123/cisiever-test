using AutoFixture;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.TestFactories.ReportSchedules;

public class IReportExecutionPlanFactory : ObjectFactory<IReportExecutionPlan>
{
    public IReportExecutionPlanFactory(IObjectFactoryRegistry registry) : base(registry)
    {
    }

    public override IReportExecutionPlan Generate(string? objectName = null)
    {
        return this.Registry.Fixture.Create<IReportExecutionPlan>();
    }
}
