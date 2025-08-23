using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Template.Business.Interfaces.System
{
    public interface IRabbitMQService
    {
        void Subscribe(string queueName, AsyncEventHandler<BasicDeliverEventArgs> receivedHandler, AsyncEventHandler<ShutdownEventArgs> shutdownHandler = null, bool durable = false, bool autoAck = false);
        void Publish(string queueName, string message, bool durable = false);
    }
}
