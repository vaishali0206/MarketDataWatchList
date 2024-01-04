using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQSignalRConsumer.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RabbitMQSignalRConsumer
{
    public interface ITypedHubClient
    {
        Task Subscribe(int matchId);
        Task ReceiveMessage(string message, string json, string userId, string connectionId);
    }

    public class MessageHub : Hub<ITypedHubClient>
    {
        public static ConcurrentDictionary<string, UserConnection> userConnectionMap = new ConcurrentDictionary<string, UserConnection>();
        public static void AddOrUpdateUserConnection(string userId, string connectionId, List<int> CompanyIDs, List<CompanyDetail> companyDetails)
        {


            if (userConnectionMap.ContainsKey(userId))
            {
                UserConnection obj = userConnectionMap[userId];
                obj.ConnectionID = connectionId;
                userConnectionMap[userId] = obj;
            }
            else
            {
                UserConnection obj = new UserConnection();
                if (!string.IsNullOrWhiteSpace(connectionId))
                    obj.ConnectionID = connectionId;
                if (CompanyIDs != null)
                    obj.CompanyIDs = CompanyIDs;
                obj.companyDetails = companyDetails;
                userConnectionMap.TryAdd(userId, obj);

            }
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            // Get user ID from connection metadata
            var userId = Context.GetHttpContext().Request.Query["userid"];
            AddOrUpdateUserConnection(userId, connectionId, null,null);
            await base.OnConnectedAsync();
        }
        public void Send(string message, string json, string userId)
        {
            //  Clients.All.ReceiveMessage( message, json,userId);
            if (MessageHub.userConnectionMap.ContainsKey(userId))
            {

                UserConnection obj = MessageHub.userConnectionMap[userId];
                //  Clients.Client(connectionId).ReceiveMessage(message, json, userId);
                Clients.Client(obj.ConnectionID).ReceiveMessage(message, json, userId, obj.ConnectionID);
            }
            //  Clients.User(userId).ReceiveMessage(message, json, userId); // Sending to the specific user
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
