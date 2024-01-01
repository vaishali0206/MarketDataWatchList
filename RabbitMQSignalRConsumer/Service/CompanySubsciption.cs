using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RabbitMQSignalRConsumer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Service
{
    public class CompanySubsciption : ICompanySubsciption
    {
        private readonly ICompanyDbContext _context;
        private readonly Random _random;
        public CompanySubsciption(ICompanyDbContext context)
        {
            _context = context;
            _random = new Random();
        }


        public async Task<List<UserCompanySubscription>> SubscriptionList()
        {
            return await _context.UserCompanySubscription.ToListAsync();
        }

        public async Task<List<UserCompanySubscription>> GetCompanybyUserID(int userID)
        {
            try
            {
                List<UserCompanySubscription> lst = await _context.UserCompanySubscription.Where(x => x.UserID == userID).ToListAsync();
                return lst;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

    }
}
