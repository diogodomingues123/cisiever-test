using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class DomainException : Exception
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected DomainException(string message)
        : base(message)
    {
    }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected DomainException(Exception exception)
        : this("There was a domain problem processing the request.", exception)
    {
    }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected DomainException(string message, Exception exception)
        : base(message, exception)
    {
    }
}
