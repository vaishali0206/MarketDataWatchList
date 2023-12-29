using RabbitMQ.Client;

namespace SignalRConsumer.Services
{
    public interface IRabbitMQService
    {
        IConnection GetConnection();
    }
}
