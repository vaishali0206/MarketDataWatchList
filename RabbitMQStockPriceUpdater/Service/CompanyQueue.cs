using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RabbitMQStockPriceUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQStockPriceUpdater.Service
{
    public class CompanyQueue : ICompanyQueue
    {
        // private readonly ICompanyDbContext _context;
        private readonly ICompanyDbContextFactory _contextFactory;
        private readonly Random _random;

        public CompanyQueue(ICompanyDbContextFactory contextFactory, Random random)
        {
            _contextFactory = contextFactory;
            _random = random;
        }

        public async Task<List<UserCompanySubscription>> SubscriptionList()
        {
            // return await _context.UserCompanySubscription.ToListAsync();
            using (var context = _contextFactory.CreateContext())
            {
                return await context.UserCompanySubscription.ToListAsync();
            }
        }

        public async Task<int> GetRandomCompanyID(List<UserCompanySubscription> lst)
        {
         

            // Get a random index
            int randomIndex = _random.Next(0, lst.Count);

            // Access the object at the random index
            UserCompanySubscription randomObject = lst[randomIndex];
            return await Task.FromResult(randomObject.CompanyID);
        }
       
    }
}
