using RabbitMQSignalRConsumer.Data;

namespace RabbitMQSignalRConsumer.Service
{
    public interface IUser
    {
        public Task<Users> GetUserByUserName(string username);
    }
}
