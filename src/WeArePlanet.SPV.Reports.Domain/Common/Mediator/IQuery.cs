using MediatR;

namespace WeArePlanet.SPV.Reports.Domain.Common.Mediator;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
