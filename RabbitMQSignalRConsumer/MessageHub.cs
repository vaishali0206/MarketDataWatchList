using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitMQSignalRConsumer.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RabbitMQSignalRConsumer
{
    public interface ITypedHubClient
    {
        Task ReceiveMessage(string message, string json);
    }

    public class MessageHub : Hub<ITypedHubClient>
    {

        public void Send(string message, string json)
        {
             Clients.All.ReceiveMessage( message, json);
           // Clients.User(Context.UserIdentifier).ReceiveMessage(message, json, userId); // Sending to the specific user
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
