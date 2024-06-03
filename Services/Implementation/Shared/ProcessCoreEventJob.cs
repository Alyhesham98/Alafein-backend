using Core.Interfaces.Shared.Services;
using DTOs.Shared.Responses;
using MediatR;
using Services.Messaging;

namespace Services.Implementation.Shared
{
    public class ProcessCoreEventJob
    {
        private readonly IPublisher _publisher;
        private readonly ISender _sender;

        public ProcessCoreEventJob(IPublisher publisher, ISender sender)
        {
            _publisher = publisher;
            _sender = sender;
        }

        public async Task Process(ICoreEvent coreEvent)
        {
            await _publisher.Publish(coreEvent);
        }

        public async Task<Response<T>> Process<T>(ICommand<T> command)
        {
            return await _sender.Send(command);
        }
    }
}
