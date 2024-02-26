using AutoFixture;

using FluentAssertions;

using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

using WeArePlanet.SPV.Reports.Common.Hangfire;

using Xunit;

namespace WeArePlanet.SPV.Reports.Common.Tests.Hangfire;

public class InjectParametersJobFilterTests : PlanetTestClass
{
    private readonly InjectParametersJobFilter sut;

    public InjectParametersJobFilterTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.sut = new InjectParametersJobFilter();
    }

    [Fact]
    public void OnCreating_NoMethodParameters_DoesNotInjectAnyJobParameter()
    {
        // Arrange
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.NoParameters)));
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void OnCreating_WithOneMethodParameter_InjectsParameterUsingItsName()
    {
        // Arrange
        const string Arg = "job input";
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.WithOneParameter)), Arg);
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().HaveCount(1);

        creatingContext.Parameters.Should()
            .ContainKey("Str") // Must be in title case
            .WhoseValue.Should().Be(Arg);
    }

    [Fact]
    public void OnCreating_WithMultipleParametersMethodParameters_InjectsParametersUsingTheirName()
    {
        // Arrange
        const string Arg1 = "job input";
        const int Arg2 = 124;
        var arg3 = Guid.NewGuid();
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.WithMultipleParameters)), Arg1, Arg2, arg3);
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().HaveCount(3);

        creatingContext.Parameters.Should()
            .ContainKey("Str") // Must be in title case
            .WhoseValue.Should().Be(Arg1);

        creatingContext.Parameters.Should()
            .ContainKey("Abc") // Must be in title case
            .WhoseValue.Should().Be(Arg2.ToString());

        creatingContext.Parameters.Should()
            .ContainKey("Param3") // Must be in title case
            .WhoseValue.Should().Be(arg3.ToString());
    }

    [Fact]
    public void OnCreating_WithJobParameterAttributes_InjectsParametersUsingTheNameInTheAttribute()
    {
        // Arrange
        const string Arg1 = "job input";
        const int Arg2 = 124;
        var arg3 = Guid.NewGuid();
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.WithJobParameterAttributes)), Arg1, Arg2, arg3);
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().HaveCount(3);

        creatingContext.Parameters.Should()
            .ContainKey("Param1") // Must be in title case
            .WhoseValue.Should().Be(Arg1);

        creatingContext.Parameters.Should()
            .ContainKey("Param2") // Must be in title case
            .WhoseValue.Should().Be(Arg2.ToString());

        creatingContext.Parameters.Should()
            .ContainKey("Param3") // Must be in title case
            .WhoseValue.Should().Be(arg3.ToString());
    }

    [Fact]
    public void OnCreating_OnlyWithPerformContextAndCancellationToken_NoParameterIsInjectedInContext()
    {
        // Arrange
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.OnlyWithPerformContextAndCancellationToken)), null,
            CancellationToken.None);
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void OnCreating_WithPerformContextAndCancellationToken_WithAnotherParameter_InjectsThatParameterOnly()
    {
        // Arrange
        const string Arg = "input value";
        var job = new Job(typeof(DummyJob).GetMethod(nameof(DummyJob.WithPerformContextAndCancellationToken)), null,
            Arg, CancellationToken.None);
        var creatingContext =
            new CreatingContext(new CreateContext(new DummyJobStorage(), this.Fixture.Create<IStorageConnection>(), job,
                this.Fixture.Create<IState>()));

        // Act
        this.sut.OnCreating(creatingContext);

        // Assert
        creatingContext.Parameters.Should().HaveCount(1);
        creatingContext.Parameters.Should()
            .ContainKey("Str") // Must be in title case
            .WhoseValue.Should().Be(Arg);
    }
}

public class DummyJob
{
    public Task NoParameters()
    {
        return Task.CompletedTask;
    }

    public Task WithOneParameter(string str)
    {
        return Task.CompletedTask;
    }

    public Task WithMultipleParameters(string str, int abc, Guid param3)
    {
        return Task.CompletedTask;
    }

    public Task WithJobParameterAttributes(
        [JobParameterName("Param1")] string str,
        [JobParameterName("Param2")] int abc,
        [JobParameterName("Param3")] Guid param3)
    {
        return Task.CompletedTask;
    }

    public Task WithPerformContextAndCancellationToken(PerformContext ctx, string str,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnlyWithPerformContextAndCancellationToken(PerformContext ctx, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class DummyJobStorage : JobStorage
{
    public override IMonitoringApi GetMonitoringApi()
    {
        throw new NotImplementedException();
    }

    public override IStorageConnection GetConnection()
    {
        throw new NotImplementedException();
    }
}
