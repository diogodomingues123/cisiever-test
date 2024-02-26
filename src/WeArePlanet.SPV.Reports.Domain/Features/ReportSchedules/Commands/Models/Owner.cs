using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class Owner : ValueObject<Owner>
{
    public Owner(string? userId, string? organizationId)
    {
        this.UserId = userId;
        this.OrganizationId = organizationId;
    }

    public string?
        UserId
    {
        get;
        init;
    } // TODO UserId and OrganizationId can be null for the time being. Review once we have a plan for auth

    public string? OrganizationId { get; init; }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.UserId;
        yield return this.OrganizationId;
    }
}
