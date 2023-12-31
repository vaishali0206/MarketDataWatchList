using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQSignalRConsumer
{
    public class RabbitMQConsumer
    {
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;

        public RabbitMQConsumer(IHubContext<MessageHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public void StartConsuming()
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = "1";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            //BasicGetResult result = channel.BasicGet(queueName, autoAck: true);
            //if (result != null)
            //{
            //    var message = System.Text.Encoding.UTF8.GetString(result.Body);
            //    Console.WriteLine($"Last Message: {message}");
            //}
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Broadcast message to SignalR clients
                //   _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                _hubContext.Clients.All.ReceiveMessage(message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
