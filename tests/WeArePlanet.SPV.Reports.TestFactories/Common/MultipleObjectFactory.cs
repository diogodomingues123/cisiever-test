using Bogus;

namespace WeArePlanet.SPV.Reports.TestFactories.Common;

public abstract class MultipleObjectFactory<T> : ObjectFactory<T>
    where T : class
{
    private readonly IDictionary<string, Faker<T>> factories;

    protected MultipleObjectFactory(IObjectFactoryRegistry registry, IDictionary<string, Faker<T>> factories)
        : base(registry)
    {
        this.factories = factories;
    }

    public override T Generate(string? objectName = null)
    {
        if (objectName == null)
        {
            return this.factories[string.Empty].Generate();
        }

        return this.factories.TryGetValue(objectName, out var faker)
            ? faker.Generate()
            : throw new InvalidOperationException(
                $"No factory has been found for type [{nameof(T)}] with name [{objectName}].");
    }
}
