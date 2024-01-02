using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using RabbitMQSignalRConsumer.Models;

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
            var factory = new ConnectionFactory()
            {
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
                dynamic dy = JsonConvert.DeserializeObject<dynamic>(message);

                //  var json = JsonConvert.SerializeObject(lstCompanyIDs);

                // Broadcast message to SignalR clients
                //   _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                //  _hubContext.Clients.All.ReceiveMessage(message, json);

                //  if (MessageHub.userConnectionMap.ContainsKey(userId))
                foreach (KeyValuePair<string, UserConnection> entry in MessageHub.userConnectionMap)
                {

                    UserConnection obj = entry.Value;
                    var json = JsonConvert.SerializeObject(obj.CompanyIDs);
                    if (obj.CompanyIDs.Contains(Convert.ToInt32(dy.CompanyID)))
                    {
                        if(!string.IsNullOrWhiteSpace( obj.ConnectionID))
                        _hubContext.Clients.Client(obj.ConnectionID).ReceiveMessage(message, json, entry.Key, obj.ConnectionID);
                    }
                }
                //{

                //        string connectionId = MessageHub.userConnectionMap[userId];
                //    //  Clients.Client(connectionId).ReceiveMessage(message, json, userId);
                //    _hubContext.Clients.Client(connectionId).ReceiveMessage(message, json, userId);
                //}



            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
