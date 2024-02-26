using AutoFixture;

namespace WeArePlanet.SPV.Reports.TestFactories.Common;

public interface IObjectFactoryRegistry
{
    Fixture Fixture { get; }
    ObjectFactory<T>? GetFactory<T>();
    T Generate<T>(string? objectName = null);
    IEnumerable<T> GenerateMany<T>(string? objectName = null, int count = 5);
}
