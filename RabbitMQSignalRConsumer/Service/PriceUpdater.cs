using Microsoft.EntityFrameworkCore;
using RabbitMQSignalRConsumer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSignalRConsumer.Service
{
    public class PriceUpdater : IPriceUpdater
    {
        private readonly ICompanyDbContext _context;
        private readonly Random _random;

        public PriceUpdater(ICompanyDbContext context)
        {
            _context = context;
            _random = new Random();
        }
        public async Task<decimal> RandomePriceGenerator(int CompanyID)
        {
            decimal randomDecimal = (decimal)_random.NextDouble() * 1000;
            return await Task.FromResult(randomDecimal);
        }
        public async Task<CompanyPrice> GetCompanyID(int CompanyID)
        {
            CompanyPrice companyPrice = await _context.CompanyPrice.AsNoTracking().Where(x => x.CompanyID == CompanyID).FirstOrDefaultAsync();
            //  _context.Detach(companyPrice);
            return companyPrice;

        }
        public async Task<CompanyPrice> UpdateDatabaseAsync(CompanyPrice companyPrice)
        {
            CompanyPrice obj = await GetCompanyID(companyPrice.CompanyID);
            if (obj != null)
                companyPrice.PriceID = obj.PriceID;
            _context.AddOrUpdate(companyPrice);
            return companyPrice;

        }
    }
}
