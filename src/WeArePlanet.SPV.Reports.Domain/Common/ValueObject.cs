using System.Diagnostics.CodeAnalysis;

namespace WeArePlanet.SPV.Reports.Domain.Common;

[ExcludeFromCodeCoverage]
public abstract class ValueObject<T> : IComponentEquatable<T>
    where T : ValueObject<T>
{
    public abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(T? other)
    {
        return this.Equals(other as ValueObject<T>);
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

        return obj.GetType() == this.GetType() && this.Equals((ValueObject<T>)obj);
    }

    public override int GetHashCode()
    {
        return this.GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private bool Equals(ValueObject<T>? other)
    {
        if (other == null)
        {
            return false;
        }

        return this.GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }
}
