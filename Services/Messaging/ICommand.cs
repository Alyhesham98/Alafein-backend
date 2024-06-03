using DTOs.Shared.Responses;
using MediatR;

namespace Services.Messaging
{
    public interface ICommand<TResponse> : IRequest<Response<TResponse>>
    {
    }
}
