using FluentValidation;
using FluentValidation.Results;

using WeArePlanet.SPV.Reports.Common.Tests;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator;
using WeArePlanet.SPV.Reports.Domain.Common.Mediator.Middleware;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Common.Mediator.Middleware;

public class ValidationPipelineBehaviorTests : PlanetTestClass
{
    private readonly ValidationPipelineBehavior<DummyRequest, DummyResponse> sut;
    private readonly List<IValidator<DummyRequest>> validatorsMock;

    public ValidationPipelineBehaviorTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        this.validatorsMock = new List<IValidator<DummyRequest>>();
        this.sut = new ValidationPipelineBehavior<DummyRequest, DummyResponse>(this.validatorsMock);
    }

    [Fact]
    public async Task Handle_NoValidationFailures_CallsNextMiddleware()
    {
        // Arrange
        var request = this.ObjectFactoryRegistry.Generate<DummyRequest>();
        var response = this.ObjectFactoryRegistry.Generate<DummyResponse>();
        var validatorMock = Substitute.For<IValidator<DummyRequest>>();

        validatorMock
            .ValidateAsync(Arg.Is<ValidationContext<DummyRequest>>(req => req.InstanceToValidate == request),
                Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        this.validatorsMock.Add(validatorMock);

        // Act
        var result = await this.sut.Handle(request, () => Task.FromResult(response), CancellationToken.None);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task Handle_HasValidationFailures_ThrowsValidationResult()
    {
        // Arrange
        var request = this.ObjectFactoryRegistry.Generate<DummyRequest>();
        var response = this.ObjectFactoryRegistry.Generate<DummyResponse>();
        var validatorMock = Substitute.For<IValidator<DummyRequest>>();

        validatorMock
            .ValidateAsync(Arg.Is<ValidationContext<DummyRequest>>(req => req.InstanceToValidate == request),
                Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("a", "b") }));

        this.validatorsMock.Add(validatorMock);

        // Act
        var result = () => this.sut.Handle(request, () => Task.FromResult(response), CancellationToken.None);

        // Assert
        (await result.Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().HaveCount(1);
    }
}

public class DummyRequest : ICommand<DummyResponse>
{
    public int Field1 { get; set; }
    public string Field2 { get; set; }
}

public class DummyResponse
{
    public bool ResultField { get; set; }
}
