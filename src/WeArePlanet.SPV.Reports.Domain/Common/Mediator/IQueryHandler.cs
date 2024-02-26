using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator;

public interface IQueryHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IQuery<TResponse>
{
}
