using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Documents;

namespace WeArePlanet.SPV.Reports.Persistence.ReportSchedules.Mappers;

public static class OwnerMapperExtensions
{
    public static OwnerDocument ToMongoDocument(this Owner? owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        return new OwnerDocument { UserId = owner.UserId, OrganizationId = owner.OrganizationId };
    }

    public static Owner? ToDomainOwner(this OwnerDocument? owner)
    {
        return owner == null ? null : new Owner(owner.UserId, owner.OrganizationId);
    }
}
