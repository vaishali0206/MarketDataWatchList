using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public interface ICompanyQueue
    {
        public Task<List<UserCompanySubscription>> SubscriptionList();
        public Task<int> GetRandomCompanyID(List<UserCompanySubscription> lst);
    }
}
