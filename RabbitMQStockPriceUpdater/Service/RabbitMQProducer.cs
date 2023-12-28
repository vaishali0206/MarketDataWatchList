using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly ICompanyQueue _companyQueue;
        private readonly IPriceUpdater _priceupdater;

        public RabbitMQProducer(ICompanyQueue companyQueue, IPriceUpdater priceupdater)
        {
            _companyQueue = companyQueue;
            _priceupdater = priceupdater;
        }

        public  void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // RabbitMQ server hostname or IP address
                Port = 5672,             // Default RabbitMQ port
                UserName = "guest",      // Default RabbitMQ username
                Password = "guest"// Default RabbitMQ password
                                  // Add other configuration settings as needed
            };
            var connection = factory.CreateConnection();



            using var channel = connection.CreateModel();

            channel.QueueDeclare("students", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "students", body: body);
        }

    }
}