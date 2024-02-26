namespace WeArePlanet.SPV.Reports.TestFactories.Common;

public abstract class ObjectFactory<T> : IObjectFactory
{
    protected ObjectFactory(IObjectFactoryRegistry registry)
    {
        this.Registry = registry;
    }

    protected IObjectFactoryRegistry Registry { get; }

    public Type ObjectType => typeof(T);

    public abstract T Generate(string? objectName = null);
}
