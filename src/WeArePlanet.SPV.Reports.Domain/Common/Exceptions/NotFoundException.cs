using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException : DomainException
{
    public NotFoundException(object? identifier)
        : base($"Resource with ID [{identifier}] not found.")
    {
        this.Id = identifier;
    }

    public object? Id { get; }
}
