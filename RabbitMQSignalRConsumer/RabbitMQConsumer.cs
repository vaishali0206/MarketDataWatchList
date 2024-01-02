using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace RabbitMQSignalRConsumer
{
    public class RabbitMQConsumer
    {
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;
      //  private readonly ConcurrentDictionary<string, string> _userConnectionMap;

        public RabbitMQConsumer(IHubContext<MessageHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
          //  _userConnectionMap = userConnectionMap;
        }

        public void StartConsuming(List<int> lstCompanyIDs, string userId)
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = "StockPrice_Queue";
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

                var json = JsonConvert.SerializeObject(lstCompanyIDs);

                // Broadcast message to SignalR clients
                //   _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                  _hubContext.Clients.All.ReceiveMessage(message, json);
                // _hubContext.Clients.User( userId).ReceiveMessage(message, json,userId);
                //if ( MessageHub._userConnectionMap.TryGetValue(userId.ToString(), out string specificConnectionId))
                //{
                //    // Send the message to the specific user on the specific connection
                //     _hubContext.Clients.Client(specificConnectionId).ReceiveMessage(message, json, userId);
                //}
                //else
                //{
                //    // Handle the case where the user is not connected
                //    // You can log a message or take appropriate action
                //}
                // await hubContext.Clients.User("userId").ReceiveMessage("Message from RabbitMQ", message, "RabbitMQ");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
