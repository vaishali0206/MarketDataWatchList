using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using RabbitMQSignalRConsumer.Models;
using RabbitMQSignalRConsumer.Data;
using RabbitMQSignalRConsumer.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading.Channels;

namespace RabbitMQSignalRConsumer
{
    public class RabbitMQConsumer
    {
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;
        private IConnection _connection;
        private IModel _channel;
        //  private readonly ConcurrentDictionary<string, string> _userConnectionMap;
        public RabbitMQConsumer(IHubContext<MessageHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672

            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            //  _userConnectionMap = userConnectionMap;
        }

        public void StartConsuming(List<int> lstCompanyIDs, string userId)
        {
            //var factory = new ConnectionFactory()
            //{
            //    HostName = "localhost",
            //    UserName = "guest",
            //    Password = "guest",
            //    Port = 5672
            //};
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            var queueName = "StockPrice_Queue";
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            //BasicGetResult result = channel.BasicGet(queueName, autoAck: true);
            //if (result != null)
            //{
            //    var message = System.Text.Encoding.UTF8.GetString(result.Body);
            //    Console.WriteLine($"Last Message: {message}");
            //}
            consumer.Received +=async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                CompanyDetail dy = JsonConvert.DeserializeObject<CompanyDetail>(message);
                // Broadcast message to SignalR clients
                //   _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                //  _hubContext.Clients.All.ReceiveMessage(message, json);
                foreach (KeyValuePair<string, UserConnection> entry in MessageHub.userConnectionMap)
                {

                    UserConnection obj = entry.Value;
                    var jsonCompanyID = JsonConvert.SerializeObject(obj.CompanyIDs);
                    if (obj.CompanyIDs.Contains(Convert.ToInt32(dy.CompanyID)))
                    {
                        var data = obj.companyDetails.Where(x => x.CompanyID == dy.CompanyID).Select(x=>x).FirstOrDefault();
                        dy.CompanyName = data.CompanyName;
                        dy.CompanyCode = data.CompanyCode;
                        message = JsonConvert.SerializeObject(dy);
                        if (!string.IsNullOrWhiteSpace( obj.ConnectionID))
                        _hubContext.Clients.Client(obj.ConnectionID).ReceiveMessage(message, jsonCompanyID, entry.Key, obj.ConnectionID);
                    }
                }

            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
