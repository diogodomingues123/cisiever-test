using AutoFixture;
using AutoFixture.AutoNSubstitute;

using WeArePlanet.SPV.Reports.TestFactories.Common;

namespace WeArePlanet.SPV.Reports.Common.Tests;

public abstract class PlanetTestClass
{
    protected PlanetTestClass(ITestOutputHelper outputHelper)
    {
        this.Fixture = new Fixture();
        this.Fixture.Customize(new AutoNSubstituteCustomization());

        this.OutputHelper = outputHelper;
        this.ObjectFactoryRegistry = new ObjectFactoryRegistry(this.Fixture);
    }

    protected IObjectFactoryRegistry ObjectFactoryRegistry { get; }
    protected Fixture Fixture { get; }
    protected ITestOutputHelper OutputHelper { get; }
}
