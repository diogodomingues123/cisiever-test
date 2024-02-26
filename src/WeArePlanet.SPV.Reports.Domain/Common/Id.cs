namespace WeArePlanet.SPV.Reports.Domain.Common;

public abstract class Id<TEntity, TId> : ValueObject<Id<TEntity, TId>>, IId<TEntity, TId>
    where TEntity : IEntity<TEntity>
{
    protected Id(TId value)
    {
        this.Value = value;
    }

    public TId? Value { get; }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }

    public override string? ToString()
    {
        return this.Value?.ToString();
    }
}
