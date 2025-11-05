using MassTransit;
using Shared.Events;

namespace UrlShortener.API.RabbitMq.Consumers
{
    public class GrpcCallSuccessfullyProcessedConsumer : IConsumer<GrpcCallProcessed>
    {
        public Task Consume(ConsumeContext<GrpcCallProcessed> context)
        {
            var message = context.Message;
            // Process the message here
            Console.WriteLine($"Received message with ID: {message.Id}");
            return Task.CompletedTask;
        }
    }
}
