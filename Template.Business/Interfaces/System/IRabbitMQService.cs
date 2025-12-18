using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Template.Business.Interfaces.System
{
    public interface IRabbitMQService
    {
        Task PublishAsync(string queueName, string message, bool durable = false);

        Task SubscribeAsync(
            string queueName,
            AsyncEventHandler<BasicDeliverEventArgs> receivedHandler,
            AsyncEventHandler<ShutdownEventArgs>? shutdownHandler = null,
            bool durable = false,
            bool autoAck = false);

        ValueTask BasicAckAsync(ulong deliveryTag, bool multiple = false);
        ValueTask BasicNackAsync(ulong deliveryTag, bool multiple = false, bool requeue = false);

    }

}
