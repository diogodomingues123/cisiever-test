using System.Collections.Immutable;

using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class ReportScheduleInput : ValueObject<ReportScheduleInput>
{
    private readonly ImmutableDictionary<string, object> parameters;

    public ReportScheduleInput(IDictionary<string, object>? parameters)
    {
        this.parameters = parameters?.ToImmutableDictionary() ?? ImmutableDictionary<string, object>.Empty;
    }

    public IReadOnlyDictionary<string, object> Parameters => this.parameters.AsReadOnly();

    public T? GetParameter<T>(string name, Func<object?, T?>? selector = null)
    {
        if (!this.parameters.TryGetValue(name, out var value))
        {
            return default;
        }

        if (value is T castValue)
        {
            return castValue;
        }

        if (selector != null)
        {
            return selector.Invoke(value);
        }

        throw new InvalidOperationException($"Parameter cannot be cast to type {typeof(T).Name}.");
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        return this.Parameters.OrderBy(p => p.Key).Cast<object?>();
    }
}
