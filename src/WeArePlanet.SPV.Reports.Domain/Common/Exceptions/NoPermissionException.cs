using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class NoPermissionException : DomainException
{
    public NoPermissionException()
        : base("You don't have permission to access this resource.")
    {
    }
}
