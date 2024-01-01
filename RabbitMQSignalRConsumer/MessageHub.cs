using Microsoft.AspNetCore.SignalR;

namespace RabbitMQSignalRConsumer
{
    public interface ITypedHubClient
    {
        Task ReceiveMessage( string message, string json);
    }

    public class MessageHub : Hub<ITypedHubClient>
    {
        public void Send( string message,string json)
        {
            Clients.All.ReceiveMessage( message, json);
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
