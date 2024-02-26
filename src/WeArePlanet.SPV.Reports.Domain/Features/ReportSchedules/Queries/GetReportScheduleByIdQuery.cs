// ReSharper disable once CheckNamespace Partial class to the generated code

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Models;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Queries.Requests;

public class GetReportScheduleByIdQuery : IQuery<ReportScheduleProjection>,
    IComponentEquatable<GetReportScheduleByIdQuery>
{
    public GetReportScheduleByIdQuery(ReportScheduleId id)
    {
        this.Id = id;
    }

    public ReportScheduleId Id { get; }


    public IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Id;
    }

    public bool Equals(GetReportScheduleByIdQuery? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((GetReportScheduleByIdQuery)obj);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}
