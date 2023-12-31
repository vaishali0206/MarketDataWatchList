using Microsoft.AspNetCore.SignalR;

namespace RabbitMQSignalRConsumer
{
    public interface ITypedHubClient
    {
        Task ReceiveMessage( string message);
    }

    public class MessageHub : Hub<ITypedHubClient>
    {
        public void Send(string name, string message)
        {
            Clients.All.ReceiveMessage( message);
        }

    }
    //public class MessageHub : Hub
    //{
    //    public async Task SendMessage(string message)
    //    {
    //        await Clients.All.SendAsync("ReceiveMessage", message);
    //    }
    //}
}
