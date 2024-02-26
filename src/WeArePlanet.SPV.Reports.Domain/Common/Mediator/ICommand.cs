using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface ICommand : ICommand<Unit>
{
}
