using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNet.SignalR;

namespace SignalRConsumer.Services
{
   

    public class MessageHandler
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageHandler(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void HandleMessage(string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
