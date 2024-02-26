using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

namespace WeArePlanet.SPV.Reports.Web.Extensions;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        return validationResult.Errors.ToProblemDetails();
    }

    public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationFailure> validationFailures)
    {
        var messages = validationFailures
            .GroupBy(error => error.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

        return new ValidationProblemDetails(messages);
    }
}
