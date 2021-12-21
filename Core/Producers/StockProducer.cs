using System;
using System.Text;
using System.Text.Json;
using DAL.Models.Mongo;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Task = System.Threading.Tasks.Task;

namespace Core.Producers
{
    public class StockProducer
    {
        public static async Task ProduceAsync(Stock stock)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "test", Password = "test", };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: "stockMessages",
                false,
                false,
                false,
                null);
            
            string message = JsonSerializer.Serialize(stock);
            var body = Encoding.UTF8.GetBytes(message);
            
            channel.BasicPublish(exchange: "",
                "stockMessages",
                null,
                body);
        }
    }
}