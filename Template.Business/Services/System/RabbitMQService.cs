using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Template.Business.Interfaces.System;
using Template.Library.Models.POCO;

namespace Template.Business.Services.System
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        public IChannel? Channel { get; private set; }

        public RabbitMQService(IOptions<RabbitMQSettings> rabbitMQConfiguration)
        {
            _factory = new ConnectionFactory
            {
                HostName = rabbitMQConfiguration.Value.RabbitMQUrl,
                UserName = rabbitMQConfiguration.Value.Username,
                Password = rabbitMQConfiguration.Value.Password,
                VirtualHost = rabbitMQConfiguration.Value.VirtualHost,
                Port = rabbitMQConfiguration.Value.Port,
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };
        }

        /* ------------------ PUBLISH ------------------ */

        public async Task PublishAsync(string queueName, string message, bool durable = false)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body);
        }

        /* ------------------ SUBSCRIBE ------------------ */

        public async Task SubscribeAsync(
            string queueName,
            AsyncEventHandler<BasicDeliverEventArgs> receivedHandler,
            AsyncEventHandler<ShutdownEventArgs>? shutdownHandler = null,
            bool durable = false,
            bool autoAck = false)
        {
            _connection = await _factory.CreateConnectionAsync();
            Channel = await _connection.CreateChannelAsync();

            await Channel.BasicQosAsync(0, 1, false);

            await Channel.QueueDeclareAsync(
                queue: queueName,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += receivedHandler;

            if (shutdownHandler != null)
                consumer.ShutdownAsync += shutdownHandler;

            await Channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: autoAck,
                consumer: consumer);
        }

        /* ------------------ ACK / NACK ------------------ */

        public ValueTask BasicAckAsync(ulong deliveryTag, bool multiple = false)
        {
            if (Channel == null)
                throw new InvalidOperationException("Channel not initialized");

            return Channel.BasicAckAsync(deliveryTag, multiple);
        }

        public ValueTask BasicNackAsync(ulong deliveryTag, bool multiple = false, bool requeue = false)
        {
            if (Channel == null)
                throw new InvalidOperationException("Channel not initialized");

            return Channel.BasicNackAsync(deliveryTag, multiple, requeue);
        }

    }
}
