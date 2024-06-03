using DTOs.Shared.Responses;
using MediatR;

namespace Services.Messaging
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, Response<TResponse>>
                                                            where TRequest : ICommand<TResponse>
    {
    }
}
