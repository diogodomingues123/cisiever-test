using System.Collections.Immutable;
using System.Reflection;

using AutoFixture;

namespace WeArePlanet.SPV.Reports.TestFactories.Common;

public class ObjectFactoryRegistry : IObjectFactoryRegistry
{
    private static readonly IReadOnlyCollection<Type> ObjectFactoryTypes;
    private readonly List<IObjectFactory> objectFactories;

    static ObjectFactoryRegistry()
    {
        ObjectFactoryTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IObjectFactory)))
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .ToImmutableList();
    }

    public ObjectFactoryRegistry(Fixture fixture)
    {
        this.Fixture = fixture;
        this.objectFactories = ObjectFactoryTypes
            .Select(type => (IObjectFactory)Activator.CreateInstance(type, this)!)
            .ToList();
    }

    public Fixture Fixture { get; }

    public ObjectFactory<T>? GetFactory<T>()
    {
        var concreteFactory = (ObjectFactory<T>?)this.objectFactories
            .FirstOrDefault(factory => factory.ObjectType == typeof(T));

        if (concreteFactory != null)
        {
            return concreteFactory;
        }

        var baseFactory = (ObjectFactory<T>?)this.objectFactories
            .FirstOrDefault(factory => typeof(T).IsAssignableTo(factory.ObjectType));

        return baseFactory;
    }

    public T Generate<T>(string? objectName = null)
    {
        var factory = this.GetFactory<T>();
        return factory != null ? factory.Generate(objectName) : this.Fixture.Create<T>();
    }

    public IEnumerable<T> GenerateMany<T>(string? objectName = null, int count = 5)
    {
        var factory = this.GetFactory<T>();
        return Enumerable.Range(0, count)
            .Select(_ => factory != null ? factory.Generate(objectName) : this.Fixture.Create<T>());
    }
}
