using RabbitMQ.Client;
using SignalRConsumer.Models;

namespace SignalRConsumer.Services
{
    public class RabbitMQService:IRabbitMQService
    {
        private readonly IConnection _connection;
        public RabbitMQService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                //HostName = configuration["RabbitMq:HostName"],
                //UserName = configuration["RabbitMq:UserName"],
                //Password = configuration["RabbitMq:Password"]

                 HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            _connection = factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }
        
    }
}
