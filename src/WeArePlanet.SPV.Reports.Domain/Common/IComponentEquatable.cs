namespace WeArePlanet.SPV.Reports.Domain.Common;

public interface IComponentEquatable<T> : IEquatable<T>
    where T : IComponentEquatable<T>
{
    protected IEnumerable<object?> GetEqualityComponents();
}
