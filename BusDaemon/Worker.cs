using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using DAL.Interfaces;
using DAL.Models.Mongo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Task = System.Threading.Tasks.Task;

namespace BusDaemon
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IMongoDbSettings _settings { get; set; }

        public Worker(ILogger<Worker> logger, IMongoDbSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Consume();
                await Task.Delay(1000, stoppingToken);
            }
        }

        protected async Task Consume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "test", Password = "test" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "stockMessages",
                    false,
                    false,
                    false,
                    null);

                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += OnConsumerOnReceived;
                
                channel.BasicConsume(queue: "stockMessages",
                    true,
                    consumer);
            }
        }

        private async void OnConsumerOnReceived(object model, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var mongoClient = new MongoClient(_settings.ConnectionString);
            var stock = JsonSerializer.Deserialize<Stock>(message);
            var db = mongoClient.GetDatabase(_settings.DatabaseName);
            using (var session = await mongoClient.StartSessionAsync())
            {
                try
                {
                    // здесь нужны транзакции serializable уровня изоляции
                    //session.StartTransaction();
                    var stocks = db.GetCollection<Stock>(nameof(Stock));
                    await stocks.InsertOneAsync(stock);
                    //await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    //await session.AbortTransactionAsync();
                }
            }

            _logger.LogInformation($"Worker recieved {message}");
        }
    }
}