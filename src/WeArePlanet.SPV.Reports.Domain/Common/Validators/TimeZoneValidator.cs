using FluentValidation;

namespace WeArePlanet.SPV.Reports.Domain.Common.Validators;

public class TimeZoneValidator : AbstractValidator<TimeZoneId>
{
    public TimeZoneValidator()
    {
        this.RuleFor(timeZone => timeZone.Value)
            .NotEmpty()
            .WithMessage("Time Zone Name cannot be empty.")
            .DependentRules(() =>
            {
                this.RuleFor(timeZone => timeZone.Value)
                    .Custom((timeZoneName, ctx) =>
                    {
                        try
                        {
                            _ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                        }
                        catch (TimeZoneNotFoundException)
                        {
                            ctx.AddFailure($"Time Zone [{timeZoneName}] not found.");
                        }
                        catch (Exception ex)
                        {
                            ctx.AddFailure(ex.Message);
                        }
                    });
            })
            .When(timeZone => timeZone != null);
    }
}
