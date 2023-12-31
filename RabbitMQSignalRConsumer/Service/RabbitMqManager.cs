using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Service
{
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using Newtonsoft.Json;

    public class RabbitMqManager:IRabbitMqManager
    {
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqManager(string hostName,int port, string userName, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port
                
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void DeclareQueue(string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }
        public void PublishMessage<T>(string queueName, T message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent '{message}' to {queueName}");
        }

        public void ConsumeMessages(string queueName, Action<string> messageHandler)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messageHandler?.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }

}
