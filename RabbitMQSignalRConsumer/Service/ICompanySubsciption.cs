using RabbitMQSignalRConsumer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Service
{
    public interface ICompanySubsciption
    {
        public Task<List<UserCompanySubscription>> SubscriptionList();
        public Task<List<UserCompanySubscription>> GetCompanybyUserID(int userID);
    }
}
