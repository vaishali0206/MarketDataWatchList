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
        private readonly IRabbitMqManager _rabbitMqManager;

        public RabbitMQProducer(IRabbitMqManager rabbitMqManager)
        {
            _rabbitMqManager = rabbitMqManager;
        }

        public void SendMessage<T>(T message, string queuename)
        {
            _rabbitMqManager.DeclareQueue(queuename);

            // Publish messages to queues
            _rabbitMqManager.PublishMessage(queuename, message);

            // Consume messages from queues
            //_rabbitMqManager.ConsumeMessages("Queue1", message => Console.WriteLine($"Received from Queue1: {message}"));

        }
        //{
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "localhost", // RabbitMQ server hostname or IP address
        //        Port = 5672,             // Default RabbitMQ port
        //        UserName = "guest",      // Default RabbitMQ username
        //        Password = "guest"// Default RabbitMQ password
        //                          // Add other configuration settings as needed
        //    };
        //    var connection = factory.CreateConnection();



        //    using var channel = connection.CreateModel();

        //    channel.QueueDeclare("students", exclusive: false);

        //    var json = JsonConvert.SerializeObject(message);
        //    var body = Encoding.UTF8.GetBytes(json);

        //    channel.BasicPublish(exchange: "", routingKey: "students", body: body);
    }


}