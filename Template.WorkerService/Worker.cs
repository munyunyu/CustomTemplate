using AutoMapper;
using RabbitMQ.Client.Events;
using System.Text;
using Template.Business.Interfaces.System;
using Template.Library.Constants;

namespace Template.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMQService rabbitMQService;

        public Worker(ILogger<Worker> logger, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            this.rabbitMQService = rabbitMQService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {


            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                rabbitMQService.Subscribe(queueName: RabbitQueue.GeneralEmailNotification, receivedHandler: Consumer_Received, shutdownHandler: null, durable: true, autoAck: false);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }

        public async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var brandId = Encoding.UTF8.GetString(body);

                _logger.LogInformation("RabbitMQ parameter: {brandId}", brandId);
                
                throw new Exception("Error");

                rabbitMQService.BasicAck(e.DeliveryTag, false);

                await Task.Yield();
            }
            catch (Exception ex)
            {
                rabbitMQService.BasicNack(e.DeliveryTag, false, false);
                
                _logger.LogError(ex, "RabbitMQ parameter: {parameter}", e.Body.ToArray());
            }
        }
    }
}
