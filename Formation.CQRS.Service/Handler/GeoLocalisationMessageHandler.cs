using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Formation.CQRS.Service.AccesLayer;
using Formation.CQRS.Service.Entity;
using Formation.CQRS.Service.Factory;
using Formation.CQRS.Service.Model;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace Formation.CQRS.Service.Handler
{
    public class GeoLocalisationMessageHandler : BackgroundService
    {
        private const string EXCHANGE_NAME = "cqrs_xchange";
        private const string ROUTING_KEY = "coordonnees";

        private readonly ILogger<GeoLocalisationMessageHandler> _logger;
        private readonly IGeoLocalisationContext _context;
        private readonly IGeoLocalisationFactory _factory;
        private readonly IConnectionFactory _rabbitmq;

        private IConnection connection;
        private IModel channel;
        private string queueName;

        public GeoLocalisationMessageHandler(
            ILogger<GeoLocalisationMessageHandler> logger,
            IConnectionFactory rabbitmq,
            IGeoLocalisationContext context,
            IGeoLocalisationFactory factory)
        {
            _logger = logger;
            _rabbitmq = rabbitmq;
            _context = context;
            _factory = factory;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            connection = _rabbitmq.CreateConnection();
            channel = connection.CreateModel();
            
            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(
                queue: queueName,
                exchange: EXCHANGE_NAME,
                routingKey: ROUTING_KEY,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                _logger.LogInformation("Message received!");

                var body = ea.Body;
                var content = Encoding.UTF8.GetString(body.ToArray());

                HandleMessage(content);

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                _logger.LogInformation("Message ack!!!");
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            _logger.LogInformation("Handler ExecuteAsync completed...");
        
            return Task.CompletedTask;
        }

        public void HandleMessage(string content)
        {
            var geoModel = JsonSerializer.Deserialize<GeoLocalisationModel>(content);
            var entity = _factory.ToEntities(geoModel);

            _context.GeoLocalisation.Add(entity);
            _context.SaveChanges();
        }
    }
}
