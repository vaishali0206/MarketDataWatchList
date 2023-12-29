
//using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRConsumer.Models;
using System;
using System.Text;


namespace SignalRConsumer.Services
{
   

    public class RabbitMQMessageConsumer : IMessageConsumer
    {
       
        private readonly RabbitMQ.Client.IConnection _connection;
        private readonly IHubContext<MessageHub> _hubContext;

      

        private IModel _channel;

        public RabbitMQMessageConsumer(IRabbitMQService connectionProvider, IHubContext<MessageHub> hubContext)
        {
            _connection = connectionProvider.GetConnection();
            _channel = _connection.CreateModel();
            _hubContext = hubContext;
        }

        public void StartReceiving(Action<string> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //  onMessageReceived.Invoke(message);
                // var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                _hubContext.Clients.All.sendMessage(message);
            };

            _channel.BasicConsume(queue: "1", autoAck: true, consumer: consumer);
        }
    }
}
