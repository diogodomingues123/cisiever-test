namespace WeArePlanet.SPV.Reports.Domain.Common;

public interface IId<TEntity>
{
    object? Value { get; }
}

public interface IId<TEntity, out TId> : IId<TEntity>
{
    new TId? Value { get; }
    object? IId<TEntity>.Value => this.Value;
}
