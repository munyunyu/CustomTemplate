using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Library.Models.POCO;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Template.Business.Services.System
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;
        public IModel Channel { get; set; }
        public RabbitMQService(IOptions<RabbitMQSettings> rabbitMQConfiguration)
        {
            _factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfiguration.Value.RabbitMQUrl,
                UserName = rabbitMQConfiguration.Value.Username,
                Password = rabbitMQConfiguration.Value.Password,
                VirtualHost = rabbitMQConfiguration.Value.VirtualHost,
                Port = rabbitMQConfiguration.Value.Port,
                DispatchConsumersAsync = true
            };
        }

        public void Publish(string queueName, string message, bool durable = false)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void Subscribe(string queueName, AsyncEventHandler<BasicDeliverEventArgs> receivedHandler, AsyncEventHandler<ShutdownEventArgs> shutdownHandler = null, bool durable = false, bool autoAck = false)
        {
            _factory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
            var connection = _factory.CreateConnection();
            Channel = connection.CreateModel();

            Channel.BasicQos(0, 1, false);
            Channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += receivedHandler;
            if (shutdownHandler != null) consumer.Shutdown += shutdownHandler;

            Channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
        }

        public void BasicNack(ulong deliveryTag, bool multiple = false, bool requeue = false)
        {
            Channel.BasicNack(deliveryTag, multiple, requeue);
        }

        public void BasicAck(ulong deliveryTag, bool multiple = false)
        {
            Channel.BasicAck(deliveryTag, multiple);
        }
    }
}
