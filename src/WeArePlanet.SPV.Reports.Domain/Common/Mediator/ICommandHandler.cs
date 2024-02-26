using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
