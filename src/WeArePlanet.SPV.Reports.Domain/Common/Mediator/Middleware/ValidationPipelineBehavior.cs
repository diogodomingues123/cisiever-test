using FluentValidation;

using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator.Middleware;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> requestValidators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> requestValidators)
    {
        this.requestValidators = requestValidators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!this.requestValidators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationTasks = this.requestValidators
            .Select(validator => validator.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validationTasks);

        var validationFailures = validationResults
            .SelectMany(validatorResult => validatorResult.Errors)
            .Where(failure => failure != null)
            .ToList();

        if (validationFailures.Count > 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}
